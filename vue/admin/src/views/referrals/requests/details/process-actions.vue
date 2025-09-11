<template>
  <div class="process-actions">
    <!-- 待处理状态 -->
    <StatusSection :request="referrerRequest" />
  </div>
</template>

<script setup lang="ts">
  import { ReferrerRequestDto } from '@/api/services'
  import StatusSection from './StatusSection.vue'

  defineProps<{
    referrerRequest: ReferrerRequestDto
  }>()
</script>
