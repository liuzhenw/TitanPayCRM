import request from '@/utils/http'

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

export class AccountService {
  static apiUrl = '/api/admin/account'

  static get() {
    return request.get<UserWithDetailsDto>({
      url: this.apiUrl
    })
  }

  static changePassword(input: ChangePasswordInput) {
    return request.put<void>({
      url: `${this.apiUrl}/password`,
      data: input
    })
  }
}
