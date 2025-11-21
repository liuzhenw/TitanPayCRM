<template>
  <el-card shadow="never">
    <el-tabs>
      <el-tab-pane label="下级用户">
        <Descendants :user-id="userIdValue" />
      </el-tab-pane>
      <el-tab-pane label="上级用户" lazy>
        <Ancestors :user-id="userIdValue" />
      </el-tab-pane>
    </el-tabs>
  </el-card>
</template>

<script setup lang="ts">
  import Descendants from '@/views/referrals/referrers/details/descendants.vue'
  import Ancestors from '@/views/referrals/referrers/details/ancestors.vue'
  const { userId } = defineProps<{
    userId: string
  }>()

  const userIdValue = ref(userId)
  watchEffect(() => {
    userIdValue.value = userId
  })
</script>

<style scoped lang="scss"></style>
