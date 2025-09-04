import request from '@/utils/http'
import { ProductSaleLogDto, ProductSaleLogQueryInput } from './types'

export class ProductSaleLogService {
  static apiUrl = '/api/admin/products/sale-logs'

  static getList(input: ProductSaleLogQueryInput) {
    return request.get<Api.Query.PagedResult<ProductSaleLogDto>>({
      url: this.apiUrl,
      params: input
    })
  }
}
