<template>
  <el-tag :type="type">{{ name }}</el-tag>
</template>

<script setup lang="ts">
  import { ReferrerRequestStatus } from '@/api/services'

  const { value } = defineProps<{
    value: ReferrerRequestStatus
  }>()

  const name = computed(() => {
    switch (value) {
      case 'pending':
        return '待处理'
      case 'approved':
        return '已通过'
      case 'rejected':
        return '已拒绝'
      case 'disabled':
        return '已禁止'
      default:
        return value
    }
  })

  const type = computed(() => {
    switch (value) {
      case 'pending':
        return 'primary'
      case 'approved':
        return 'success'
      case 'rejected':
        return 'danger'
      case 'disabled':
        return 'warning'
      default:
        return 'info'
    }
  })
</script>

<style scoped lang="scss"></style>
