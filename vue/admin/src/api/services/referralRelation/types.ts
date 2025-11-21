import { ReferralLevelBasicDto, ReferrerSaleStatisticDto, UserBasicDto } from '..'

export interface ReferralRelationDto {
  recommender: ReferralRelationUserDto
  recommendee: ReferralRelationUserDto
  depth: number
  createdAt: string
}

export interface ReferralRelationUserDto {
  id: string
  email: string
}

export interface ReferralRelationCreateInput {
  recommenderId: string
  recommendeeId: string
}

export interface ReferralRelationQueryInput extends Api.Query.PagedRequestBase {
  recommenderId?: string
  recommendeeId?: string
  minDepth?: number
}

export interface RecommendeeDto {
  /** @format uuid */
  id: string
  email: string
  totalConsumption: number
  consumptionCount: number
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

export interface RecommendeeQueryInput extends Api.Query.PagedRequestBase {
  /** @format uuid */
  recommenderId?: string
  levelId?: string
  /** @format uint32 */
  depth?: number
}

export interface AncestorDto {
  user: UserBasicDto
  depth: number
  level?: ReferralLevelBasicDto
  /** @format double */
  totalCommission: number
  statistics?: ReferrerSaleStatisticDto[]
  /** @format date-time */
  createdAt: string
}
