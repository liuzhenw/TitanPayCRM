<template>
  <el-form label-width="120px">
    <SettingItem v-for="item in settings" :value="item"> </SettingItem>
  </el-form>
</template>

<script setup lang="ts">
  import { SettingItemDto } from '@/api/services/settings/types'
  import SettingItem from './item.vue'

  defineOptions({ name: 'SettingGroup' })
  const { items } = defineProps<{
    items: SettingItemDto[]
  }>()
  const settings = ref<SettingItemDto[]>([])
  watchEffect(() => {
    settings.value = [...items]
  })
</script>

<style scoped lang="scss"></style>
