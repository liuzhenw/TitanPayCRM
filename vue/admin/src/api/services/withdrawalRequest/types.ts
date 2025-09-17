import { UserBasicDto } from '../index'

export type WithdrawalRequestStatus = 'pending' | 'approved' | 'rejected'

export interface WithdrawalRequestDto {
  /** @format uuid */
  id: string
  referrer: UserBasicDto
  status: WithdrawalRequestStatus
  /** @format double */
  amount: number
  fee: number
  toAddress: string
  txid?: string
  rejectReason?: string
  /** @format date-time */
  completedAt?: string
  /** @format date-time */
  createdAt: string
}

export interface WithdrawalRequestQueryInput extends Api.Query.PagedRequestBase {
  /** @format uuid */
  referrerId?: string
  status?: WithdrawalRequestStatus
  toAddress?: string
  txid?: string
}

export interface WithdrawalRequestApproveInput {
  txid: string
}

export interface WithdrawalRequestRejectInput {
  reason: string
}
