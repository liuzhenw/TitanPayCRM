import { ProductBasicDto, ReferralLevelBasicDto, UserBasicDto } from '../index'

export interface ReferrerDto {
  /** @format uuid */
  id: string
  user: UserBasicDto
  level?: ReferralLevelBasicDto
  /** @format int32 */
  directCount?: number
  /** @format int32 */
  indirectCount?: number
  /** @format int32 */
  totalCount?: number
  /** @format double */
  commission?: number
  /** @format double */
  withdrawal?: number
  withdrawalAddress?: string | null
  isDisabled?: boolean
  remark?: string | null
  /** @format date-time */
  updatedAt?: string | null
  /** @format date-time */
  createdAt?: string
}

export interface ReferrerSaleStatisticDto {
  /** @format uuid */
  id?: string
  product: ProductBasicDto
  /** @format int32 */
  volume?: number
  /** @format double */
  revenue?: number
  /** @format double */
  commission?: number
  /** @format date-time */
  updatedAt?: string | null
  /** @format date-time */
  createdAt?: string
}

export interface ReferrerWithDetailsDto extends ReferrerDto {
  statistics: ReferrerSaleStatisticDto[]
}

export interface ReferrerUpdateInput {
  levelId?: string | null
  isDisabled: boolean
  remark?: string | null
}

export interface ReferrerQueryInput extends Api.Query.PagedRequestBase {
  id?: string
  levelId?: string
}
