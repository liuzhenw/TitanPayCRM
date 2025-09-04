import { ProductBasicDto, UserBasicDto } from '../index'

export interface ProductSaleLogDto {
  id: string
  product: ProductBasicDto
  customer: UserBasicDto
  orderNo: string
  quantity: number
  amount: number
  createdAt: string
}

export interface ProductSaleLogQueryInput extends Api.Query.PagedRequestBase {
  productId?: string
  customerId?: string
  orderNo?: string
}
