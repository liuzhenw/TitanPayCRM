export interface ChangePasswordInput {
  currentPassword: string
  newPassword: string
  confirmPassword: string
}

export interface UserWithDetailsDto {
  id: string
  name: string
  email: string
  avatarUrl?: string
  avatarUri?: string
  attempts: number
  lockedAt?: string
  updatedAt?: string
  createdAt: string
  roles: string[]
}
