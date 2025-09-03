import { Culture, CultureWithDefault, Localizable } from '@/types/common/localizable'

export const buildLocalizablePropertyName = (
  languageName: CultureWithDefault,
  propertyName: string
) => {
  if (languageName === 'default') {
    return propertyName
  }

  return `localizable.${languageName}.${propertyName}`
}

export const getLocalizableProperty = (
  languageName: CultureWithDefault,
  model: any,
  propertyName: string
) => {
  if (languageName === 'default') {
    return model[propertyName]
  }

  if (!model.localizable[languageName as Culture]) {
    model.localizable[languageName as Culture] = {}
  }

  return model.localizable[languageName as Culture][propertyName]
}

export const setLocalizableProperty = (
  languageName: CultureWithDefault,
  model: any,
  propertyName: string,
  value: string
) => {
  if (languageName === 'default') {
    model[propertyName] = value
  } else {
    model.localizable[languageName as Culture][propertyName] = value
  }
}

// 创建空的本地化属性
export const emptyLocalizable = (): Localizable => {
  return {
    'en-US': {},
    'zh-Hant': {}
  }
}
