import request from '@/utils/http'
import { ProductDto, ProductCreateInput, ProductUpdateInput } from './types'

export class ProductService {
  static apiUrl = '/api/admin/products'

  static getList() {
    return request.get<ProductDto[]>({
      url: this.apiUrl
    })
  }

  static get(id: string) {
    return request.get<ProductDto>({
      url: `${this.apiUrl}/${id}`
    })
  }

  static create(input: ProductCreateInput) {
    return request.post<ProductDto>({
      url: this.apiUrl,
      data: input
    })
  }

  static update(id: string, input: ProductUpdateInput) {
    return request.put<ProductDto>({
      url: `${this.apiUrl}/${id}`,
      data: input
    })
  }

  static delete(id: string) {
    return request.del<void>({
      url: `${this.apiUrl}/${id}`
    })
  }

  static async getOptions() {
    const products = await this.getList()
    return products.map((p) => ({ label: p.name, value: p.id }))
  }
}
