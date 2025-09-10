<template>
  <el-tag :type="tagType">{{ statusText }}</el-tag>
</template>

<script setup lang="ts">
  import { computed } from 'vue'
  import { WithdrawalRequestStatus } from '@/api/services'

  const props = defineProps<{
    value: WithdrawalRequestStatus
  }>()

  const statusText = computed(() => {
    switch (props.value) {
      case 'pending':
        return '待处理'
      case 'approved':
        return '已批准'
      case 'rejected':
        return '已拒绝'
      default:
        return props.value
    }
  })

  const tagType = computed(() => {
    switch (props.value) {
      case 'pending':
        return 'warning'
      case 'approved':
        return 'success'
      case 'rejected':
        return 'danger'
      default:
        return 'info'
    }
  })
</script>

<style scoped lang="scss"></style>