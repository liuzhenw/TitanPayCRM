/**
 * namespace: Api
 *
 * 所有接口相关类型定义
 * 在.vue文件使用会报错，需要在 eslint.config.mjs 中配置 globals: { Api: 'readonly' }
 */
declare namespace Api {
  namespace Query {
    // 分页查询参数
    export interface PagedRequestBase {
      sorting?: string
      skipCount?: number
      maxResultCount?: number
    }

    // 列表查询结果
    export interface ListResult<T> {
      items: T[]
    }

    // 分页查询结果
    export interface PagedResult<T> extends ListResult<T> {
      totalCount: number
    }
  }

  namespace Accounts {
    export interface AccountInfo {
      id: string
      name: string
      email: string
      avatarUri?: string
      roles: string[]
    }

    export interface AccessToken {
      accessToken: string
      refreshToken?: string
      expiresIn: number
    }
  }
}
