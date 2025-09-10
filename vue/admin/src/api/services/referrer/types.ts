import { ProductBasicDto, ReferralLevelBasicDto, UserBasicDto } from '../index'

export interface ReferrerDto {
  /** @format uuid */
  id: string
  user: UserBasicDto
  level?: ReferralLevelBasicDto
  /** @format int32 */
  directCount: number
  /** @format int32 */
  indirectCount: number
  /** @format int32 */
  totalCount: number
  /** @format double */
  totalCommission: number
  /** @format double */
  commission: number
  /** @format double */
  withdrawal: number
  withdrawalAddress?: string
  isDisabled: boolean
  remark?: string
  /** @format date-time */
  updatedAt?: string | null
  /** @format date-time */
  createdAt: string
}

export interface ReferrerSaleStatisticDto {
  product: ProductBasicDto
  /** @format int32 */
  volume?: number
  /** @format double */
  revenue?: number
  /** @format double */
  commission?: number
}

export interface ReferrerWithDetailsDto extends ReferrerDto {
  statistics: ReferrerSaleStatisticDto[]
}

export interface ReferrerCreateInput {
  email: string
  levelId?: string
  remark?: string
}

export interface ReferrerUpdateInput {
  levelId?: string
  isDisabled: boolean
  remark?: string
}

export interface ReferrerQueryInput extends Api.Query.PagedRequestBase {
  id?: string
  levelId?: string
}

export interface RecommendeeViewDto {
  /** @format uuid */
  id: string
  email: string
  level?: ReferralLevelBasicDto
  recommender: UserBasicDto
  ancestor: UserBasicDto
  /** @format uint32 */
  depth: number
  /** @format double */
  totalCommission: number
  statistics?: ReferrerSaleStatisticDto[]
  /** @format date-time */
  createdAt: string
}

export interface RecommendeeViewQueryInput extends Api.Query.PagedRequestBase {
  /** @format uuid */
  ancestorId?: string
  /** @format uuid */
  recommenderId?: string
  levelId?: string
  /** @format uint32 */
  depth?: number
}
