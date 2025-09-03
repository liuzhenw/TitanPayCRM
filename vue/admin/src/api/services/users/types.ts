export type UserSource = 'Platform' | 'Web3'
export interface UserBasicDto {
  id: string
  name: string
  email: string
  avatarUrl?: string
}

export interface UserDto extends UserBasicDto {
  avatarUri?: string
  attempts: number
  lockedAt?: string
  updatedAt?: string
  createdAt: string
}

export interface UserWithDetailsDto extends UserDto {
  roles: string[]
}

export interface UserQueryInput extends Api.Query.PagedRequestBase {
  name?: string
  email?: string
}

export interface UserUpdateInput {
  roles: string[]
}
