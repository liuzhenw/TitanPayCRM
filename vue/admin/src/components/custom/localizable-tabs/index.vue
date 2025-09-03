<template>
  <el-tabs v-model="tab">
    <el-tab-pane v-for="item in languages" :label="item.label" :name="item.name">
      <slot :language="item"></slot>
    </el-tab-pane>
  </el-tabs>
</template>

<script setup lang="ts">
  import { CultureWithDefault } from '@/types/common/localizable'
  import { ref } from 'vue'

  type Language = {
    label: string
    name: CultureWithDefault
  }
  defineSlots<{
    default: (props: { language: Language }) => any
  }>()

  const languages: Language[] = [
    { label: '默认语言', name: 'default' },
    { label: '繁体中文', name: 'zh-Hant' },
    { label: 'English', name: 'en-US' }
  ]
  const tab = ref(languages[0].name)
</script>

<style scoped lang="scss"></style>
