import request from '@/utils/http'

export interface LoginInput {
  userName: string
  password: string
}

export interface ChangePasswordInput {
  oldPassword: string
  newPassword: string
  confirmPassword: string
}

export class AccountService {
  static login(input: LoginInput) {
    return request.post<Api.Accounts.AccessToken>({
      url: '/api/auth/sign-in/password',
      params: input
    })
  }

  static changePassword(input: ChangePasswordInput) {
    return request.put<void>({
      url: '/api/admin/account/password',
      data: input
    })
  }

  static get() {
    return request.get<Api.Accounts.AccountInfo>({
      url: '/api/admin/account'
    })
  }
}
