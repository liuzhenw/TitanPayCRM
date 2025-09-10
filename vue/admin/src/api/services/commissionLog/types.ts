import { ProductBasicDto, ReferralLevelBasicDto, UserBasicDto } from '../index'

export interface CommissionLogDto {
  /** @format uuid */
  id: string
  product: ProductBasicDto
  receiver: UserBasicDto
  customer: UserBasicDto
  level: ReferralLevelBasicDto
  /** @format uint32 */
  referralDepth: number
  /** @format double */
  amount: number
  /** @format uuid */
  saleLogId: string
  /** @format date-time */
  createdAt: string
}

export interface CommissionLogQueryInput extends Api.Query.PagedRequestBase {
  productId?: string
  /** @format uuid */
  receiverId?: string
  levelId?: string
  /** @format uuid */
  customerId?: string
  /** @format uint16 */
  referralDepth?: number
}