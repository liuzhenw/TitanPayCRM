import { RoutesAlias } from '../routesAlias'
import { AppRouteRecord } from '@/types/router'

/**
 * 菜单列表、异步路由
 *
 * 支持两种模式:
 * 前端静态配置 - 直接使用本文件中定义的路由配置
 * 后端动态配置 - 后端返回菜单数据，前端解析生成路由
 *
 * 菜单标题（title）:
 * 可以是 i18n 的 key，也可以是字符串，比如：'用户列表'
 *
 * RoutesAlias.Layout 指向的是布局组件，后端返回的菜单数据中，component 字段需要指向 /index/index
 * 路由元数据（meta）：异步路由在 asyncRoutes 中配置，静态路由在 staticRoutes 中配置
 */
export const asyncRoutes: AppRouteRecord[] = [
  {
    name: 'Referrals',
    path: '/referrals',
    component: RoutesAlias.Layout,
    meta: {
      title: '推广管理',
      icon: '&#xe67a;'
    },
    children: [
      {
        name: 'ReferrerList',
        path: 'referrers',
        component: RoutesAlias.Referrers,
        meta: {
          title: '推广代理'
        }
      },
      {
        name: 'ReferrerRequests',
        path: 'requests',
        component: RoutesAlias.ReferrerRequests,
        meta: {
          title: '代理申请'
        }
      }
    ]
  },
  {
    name: 'Products',
    path: '/products',
    component: RoutesAlias.Layout,
    meta: {
      title: '商品管理',
      icon: '&#xe737;'
    },
    children: [
      {
        name: 'ProductList',
        path: '/products/list',
        component: RoutesAlias.Products,
        meta: {
          title: '商品列表'
        }
      },
      {
        name: 'ProductSaleLogs',
        path: '/products/sale-logs',
        component: RoutesAlias.ProductSaleLogs,
        meta: {
          title: '销售记录'
        }
      }
    ]
  },
  {
    name: 'User',
    path: '/users',
    component: RoutesAlias.Layout,
    meta: {
      title: '用户管理',
      icon: '&#xe724;'
    },
    children: [
      {
        name: 'UserList',
        path: 'index',
        component: RoutesAlias.Users,
        meta: {
          title: '用户列表'
        }
      },
      {
        name: 'UserDetails',
        path: ':id/details',
        component: '/users/list/details',
        meta: {
          title: '用户详情',
          isHide: true
        }
      },
      {
        name: 'RoleList',
        path: 'roles',
        component: RoutesAlias.Roles,
        meta: {
          title: '角色列表'
        }
      }
    ]
  },
  {
    name: 'System',
    path: '/system',
    component: RoutesAlias.Layout,
    meta: {
      title: 'menus.system.title',
      icon: '&#xe755;'
    },
    children: [
      {
        path: 'settings',
        name: 'Settings',
        component: RoutesAlias.Settings,
        meta: {
          title: '系统设置',
          keepAlive: false
        }
      }
    ]
  }
]
