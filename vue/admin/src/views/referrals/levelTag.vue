<template>
  <el-tag :type="tagType" style="width: 60px;">{{ value?.name || '无等级' }}</el-tag>
</template>

<script setup lang="ts">
  import { computed } from 'vue'
  import { ReferralLevelBasicDto } from '@/api/services'

  const { value } = defineProps<{ value: ReferralLevelBasicDto }>()
  const tagType = computed(() => {
    switch (value?.size) {
      case 1:
        return 'primary'
      case 2:
        return 'success'
      case 3:
        return 'warning'
      default:
        return 'info'
    }
  })
</script>

<style scoped lang="scss"></style>
