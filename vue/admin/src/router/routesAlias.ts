/**
 * 路由别名，方便快速找到页面，同时可以用作路由跳转
 */
export enum RoutesAlias {
  Users = '/users/list',
  UserDetails = '/users/:id/details',
  Roles = '/users/roles',

  Products = '/products/list',
  ProductSaleLogs = '/products/sale-logs',
  ProductSaleLogDetails = '/products/sale-logs/:id/details',

  Referrers = '/referrals/referrers',
  ReferrerDetails = '/referrals/referrers/:id',

  ReferrerRequests = '/referrals/requests',
  CommissionLogs = '/referrals/commission-logs',
  WithdrawalRequests = '/referrals/withdrawal-requests',
  WithdrawalRequestDetails = '/referrals/withdrawal-requests/:id',

  // 系统管理
  Blockchains = '/system/blockchains', // 区块链
  ChainTokens = '/system/chain-tokens', // 链代币
  Settings = '/system/settings', // 系统设置

  // 布局和认证
  Layout = '/static/index/index', // 布局容器
  Login = '/auth/login', // 登录
  // 异常页面
  Exception403 = '/exception/403', // 403
  Exception404 = '/exception/404', // 404
  Exception500 = '/exception/500' // 500
}
