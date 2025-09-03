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
        path: ':id',
        component: "/users/details",
        meta: {
          title: '用户详情',
          isHide: true
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
