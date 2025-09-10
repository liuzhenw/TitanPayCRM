import { ProductBasicDto, UserBasicDto } from '../index'
import { ProductDto } from '../product/types'
import { UserDto } from '../users/types'

export interface ProductSaleLogDto {
  id: string
  product: ProductBasicDto
  customer: UserBasicDto
  orderNo: string
  quantity: number
  amount: number
  totalCommission: number
  createdAt: string
}

export interface ProductSaleLogQueryInput extends Api.Query.PagedRequestBase {
  productId?: string
  customerId?: string
  orderNo?: string
}
