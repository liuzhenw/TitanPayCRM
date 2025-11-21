<template>
  <el-card shadow="never">
    <el-tabs v-model="tab">
      <el-tab-pane label="下级用户" name="descendants" lazy>
        <Descendants :user-id="userIdValue" />
      </el-tab-pane>
      <el-tab-pane label="上级用户" name="ancestors" lazy>
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
  const tab = ref('descendants')
</script>

<style scoped lang="scss"></style>
