export interface Product {
  name: string;
  created: string;
  thumbnailImage: string;
  price: {
    currency: string;
    amount: number;
  };
}
