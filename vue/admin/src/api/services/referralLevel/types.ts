import { ProductBasicDto, UserBasicDto } from '../index'

export interface ReferralLevelBasicDto {
  id?: string | null
  name?: string | null
  /** @format int32 */
  size?: number
}

export interface ReferralLevelDto extends ReferralLevelBasicDto {
  description?: string
  multiplier?: number
  /** @format int32 */
  userCount?: number
  /** @format double */
  totalCommission?: number
  /** @format date-time */
  updatedAt?: string | null
  /** @format date-time */
  createdAt?: string
}

export interface ReferralLevelCreateInput {
  /**
   * @minLength 1
   * @maxLength 32
   */
  id: string
  /**
   * @minLength 1
   * @maxLength 32
   */
  name: string
  /** @format int32 */
  size?: number
  /**
   * @format double
   * @min 0
   */
  multiplier?: number
}
export interface ReferralLevelUpdateInput {
  /**
   * @minLength 1
   * @maxLength 32
   */
  name: string
  /** @format int32 */
  size?: number
  /**
   * @format double
   * @min 0
   */
  multiplier?: number
}
