// 本地化文化类型
export type Culture = 'zh-Hant' | 'en-US'
export type CultureWithDefault = Culture | 'default'
export const Cultures: Culture[] = ['zh-Hant', 'en-US']
// 本地化内容
export type Localizable = Record<Culture, LocalizableProperty>
// 本地化属性
export interface LocalizableProperty {
  [key: string]: string
}
