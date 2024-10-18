using colengo.app.Server.Data;
using colengo.app.Server.Helper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace colengo.app.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApiContext _context;

        public ProductController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet("migrate")]
        public async Task<IActionResult> MigrateProducts()
        {
            var url = Environment.GetEnvironmentVariable("UrlProducts");

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";

                byte[] byteArray = Encoding.UTF8.GetBytes("{\"Page\":1, \"PageSize\":50}");
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                var productResponse = new ProductMasterPaging();
                // Get the response
                using (WebResponse response = request.GetResponse())
                {
                    // Read the response stream
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        productResponse = JsonConvert.DeserializeObject<ProductMasterPaging>(reader.ReadToEnd());
                    }
                }

                if (productResponse != null && productResponse.Products != null && productResponse.Products.Any())
                {
                    var products = _context.Products.ToList();

                    foreach (var product in productResponse.Products)
                    {
                        //brand
                        if (product.Brand != null)
                        {
                            var existBrand = _context.Brands?.FirstOrDefault(it => it.Name == product.Brand.Name);
                            if (existBrand == null)
                            {
                                await _context.Brands.AddAsync(product.Brand);
                                await _context.SaveChangesAsync();
                                product.BrandId = product.Brand.Id;
                            }
                            else
                                product.BrandId = existBrand.Id;
                        }

                        //product
                        var existProduct = products?.FirstOrDefault(it => it.ProductId == product.ProductId);
                        if (existProduct != null)
                        {
                            //update
                            _context.Entry(existProduct).State = EntityState.Deleted;
                            _context.Set<Product>().Update(product);
                        }
                        else
                        {
                            //create
                            await _context.Products.AddAsync(product);

                            //price
                            var existPrice = _context.ProductPrices?.FirstOrDefault(it => it.ProductId == product.ProductId);
                            if (existPrice == null)
                            {
                                await _context.ProductPrices.AddAsync(new ProductPrice()
                                {
                                    ProductId = product.ProductId,
                                    PriceCurrency = product.Price?.Currency,
                                    PriceAmount = product.Price?.Amount,
                                    OriginalPriceCurrency = product.OriginalPrice?.Currency,
                                    OriginalPriceAmount = product.OriginalPrice?.Amount,
                                    FullPriceBeforeOverallDiscountCurrency = product.FullPriceBeforeOverallDiscount?.Currency,
                                    FullPriceBeforeOverallDiscountAmount = product.FullPriceBeforeOverallDiscount?.Amount,
                                    PossibleDiscountPriceCurrency = product.PossibleDiscountPrice?.Currency,
                                    PossibleDiscountPriceAmount = product.PossibleDiscountPrice?.Amount,
                                });
                            }

                            //category
                            if (product.Categories != null && product.Categories.Any())
                            {
                                foreach (var category in product.Categories)
                                {
                                    var existCategory = _context.Categories?.FirstOrDefault(it => it.Id == category.Id);
                                    if (existCategory == null)
                                        await _context.Categories.AddAsync(category);

                                    var existProductCategory = _context.ProductCategories?.FirstOrDefault(it => it.ProductId == product.ProductId && it.CategoryId == category.Id);
                                    if (existProductCategory == null)
                                    {
                                        await _context.ProductCategories.AddAsync(new ProductCategory()
                                        {
                                            ProductId = product.ProductId,
                                            CategoryId = category.Id
                                        });
                                    }
                                }
                            }

                            //images
                            if (product.Images != null && product.Images.Any())
                            {
                                foreach (var img in product.Images)
                                {
                                    img.ProductId = product.ProductId;
                                    _context.ProductImages.AddAsync(img);
                                }
                            }

                            //image taxonomy tags
                            if (product.ImageTaxonomyTags != null && product.ImageTaxonomyTags.Any())
                            {
                                foreach (var tag in product.ImageTaxonomyTags)
                                {
                                    foreach (var img in tag.Value)
                                    {
                                        img.ProductId = product.ProductId;
                                        img.Taxonomy = tag.Key;
                                        _context.ProductImageTaxonomyTags.AddAsync(img);
                                    }
                                }
                            }

                            //tag
                            if (product.Tags != null && product.Tags.Any())
                            {
                                foreach (var tag in product.Tags)
                                {
                                    var existTag = _context.Tags?.FirstOrDefault(it => it.Id == tag.Id);
                                    if (existTag == null)
                                        await _context.Tags.AddAsync(tag);

                                    var existProductTag = _context.ProductTags?.FirstOrDefault(it => it.ProductId == product.ProductId && it.TagId == tag.Id);
                                    if (existProductTag == null)
                                    {
                                        await _context.ProductTags.AddAsync(new ProductTag()
                                        {
                                            ProductId = product.ProductId,
                                            TagId = tag.Id
                                        });
                                    }

                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    // Handle any web exceptions
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        return BadRequest(errorResponse);
                    }
                }
                else
                    return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(string? searchTerm, string? sortColumn, string? sortDirection, int? page, int? pageSize)
        {
            string orderBy = string.IsNullOrEmpty(sortColumn) ? "ProductId" : ConvertPatternString.ToPascalCase(sortColumn);

            var results = await (from it in _context.Products
                                  join p in _context.ProductPrices on it.ProductId equals p.ProductId into pr
                                  from p in pr.DefaultIfEmpty()
                                  where (true == (string.IsNullOrEmpty(searchTerm) || (it.Name != null && it.Name.Contains(searchTerm))))
                                  select new
                                  {
                                      it.ProductId,
                                      it.Name,
                                      it.Created,
                                      it.ThumbnailImage,
                                      Price = new { Currency = p.PriceCurrency, Amount = p.PriceAmount }
                                  }).OrderBy(orderBy, (string.IsNullOrEmpty(sortDirection) ? "asc" : sortDirection)).ToListAsync();

           return Ok(new ProductModelPaging()
            {
                products = results.Skip(page > 0 ? page.Value - 1 : 0).Take(pageSize > 0 ? pageSize.Value : 10),
                total = results.Count
            });
        }

        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var imgs = await _context.ProductImages.Where(o => o.ProductId == productId).ToListAsync();

            var imgTags = await _context.ProductImageTaxonomyTags.Where(o => o.ProductId == productId).
                              GroupBy(i => i.Taxonomy).Select(igrp => new
                              {
                                  igrp.Key,
                                  Data = igrp.ToList()
                              }).ToListAsync();


            var imgTagsDict = imgTags.ToDictionary(abc => abc.Key, zxc => zxc.Data.ToList());

            var product = (from it in _context.Products

                           join b in _context.Brands on it.BrandId equals b.Id into br
                           from b in br.DefaultIfEmpty()

                           join p in _context.ProductPrices on it.ProductId equals p.ProductId into pr
                           from p in pr.DefaultIfEmpty()

                           where it.ProductId == productId
                           select new
                           {
                               it.ProductId,
                               it.Name,
                               it.Title,
                               it.ThumbnailImage,
                               Price = new { Currency = p.PriceCurrency, Amount = p.PriceAmount },
                               OriginalPrice = new { Currency = p.OriginalPriceCurrency, Amount = p.OriginalPriceAmount },
                               Brand = new { b.Name, b.UserIdentifier },
                               it.Sold,
                               it.AllowMultipleConfigs,
                               it.Url,
                               it.Created,
                               it.OverallCampaignEndDate,
                               it.ReviewScore,
                               it.ReviewCount,
                               it.Has3DAssets,
                               FullPriceBeforeOverallDiscount = new { Currency = p.FullPriceBeforeOverallDiscountCurrency, Amount = p.FullPriceBeforeOverallDiscountAmount },
                               PossibleDiscountPrice = new { Currency = p.PossibleDiscountPriceCurrency, Amount = p.PossibleDiscountPriceAmount },
                               it.Layout,
                               it.Location,
                               Categories = _context.Categories.Join(_context.ProductCategories,
                                  c => c.Id,
                                  pc => pc.CategoryId,
                                  (c, pc) => new { pc.ProductId, Category = c }
                              ).Where(o => o.ProductId == it.ProductId).Select(c => c.Category).ToList(),
                               Tags = _context.Tags.Join(_context.ProductTags,
                                  t => t.Id,
                                  pt => pt.TagId,
                                  (t, pt) => new { pt.ProductId, Tag = t }
                              ).Where(o => o.ProductId == it.ProductId).Select(t => t.Tag).ToList(),
                               Images = imgs,
                               ImageTaxonomyTags = imgTagsDict
                           }).FirstOrDefaultAsync();


            if (product != null)
                return Ok(product);
            else
                return NotFound();
        }
    }
}
