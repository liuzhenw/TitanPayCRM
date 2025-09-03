<template>
  <component :is="h(ElInput, { ...attrs, ...props, ref: onRefChange }, slots)" />
</template>

<script setup lang="ts">
  import { h, getCurrentInstance } from 'vue'
  import { ElInput, InputProps } from 'element-plus'
  const attrs = useAttrs()
  const slots = useSlots()
  const props = withDefaults(defineProps<Partial<InputProps>>(), {
    formatter: (value: string | number) => {
      return String(value).replace(/[^\d]/g, '') // 显示时只保留数字
    },
    parser: (value: string) => {
      const number = value.replace(/[^\d]/g, '') // 输入时只保留数字
      return parseFloat(number)
    }
  })

  const vm = getCurrentInstance()
  const onRefChange = (instance: any) => {
    vm!.exposed = vm!.exposeProxy = instance || {}
  }
</script>

<style scoped lang="scss"></style>
