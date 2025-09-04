<template>
  <el-tabs v-loading="!user" v-model="tab">
    <el-tab-pane label="基本信息" name="basic">
      <Basic v-if="user" :value="user" />
    </el-tab-pane>
  </el-tabs>
</template>

<script setup lang="ts">
  import { UserService, UserWithDetailsDto } from '@/api/services'
  import Basic from './basic.vue'
  const route = useRoute()
  const userId = route.params.id as string
  const tab = ref<'basic' | 'invite-logs' | 'invite-reward-logs'>('basic')
  const user = ref<UserWithDetailsDto>()
  onMounted(async () => {
    user.value = await UserService.get(userId)
  })
</script>

<style scoped lang="scss"></style>
