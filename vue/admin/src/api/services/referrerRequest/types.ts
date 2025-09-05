import { ReferralLevelBasicDto, UserBasicDto } from '..'

export type ReferrerRequestStatus = 'pending' | 'approved' | 'rejected' | 'disabled'

export interface ReferrerRequestDto {
  id: string
  status: ReferrerRequestStatus
  user: UserBasicDto
  level: ReferralLevelBasicDto
  rejectReason?: string
  createdAt: string
  updatedAt?: string
}

export interface ReferrerRequestRejectInput {
  reason: string
  isDisabled: boolean
}

export interface ReferrerRequestQueryInput extends Api.Query.PagedRequestBase {
  id?: string
  status?: ReferrerRequestStatus
  levelId?: string
}
