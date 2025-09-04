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

export interface ReferralRelationQueryInput extends Api.Query.PagedRequestBase {
  recommenderId?: string
  recommendeeId?: string
  minDepth?: number
}
