<template>
  <el-form label-width="120px">
    <SettingItem v-for="item in settings" :value="item"> </SettingItem>
    <el-form-item v-if="groupName === '邮件'" label="邮件发送测试">
      <el-input v-model="testEmailReceiver" placeholder="请输入测试邮件接收人的邮箱地址" clearable>
        <template #append>
          <el-button
            type="primary"
            :loading="testEmailSending"
            :disabled="!testEmailReceiver"
            @click="sendTestEmail"
          >
            发送
          </el-button>
        </template>
      </el-input>
    </el-form-item>
  </el-form>
</template>

<script setup lang="ts">
  import { SettingItemDto } from '@/api/services/settings/types'
  import SettingItem from './item.vue'
  import { SettingService } from '@/api/services'
  import { ElMessage } from 'element-plus'

  defineOptions({ name: 'SettingGroup' })
  const { groupName, items } = defineProps<{
    groupName: string
    items: SettingItemDto[]
  }>()
  const settings = ref<SettingItemDto[]>([])

  const testEmailReceiver = ref('')
  const testEmailSending = ref(false)
  const sendTestEmail = async () => {
    testEmailSending.value = true
    await SettingService.sendTestEmail(testEmailReceiver.value).finally(
      () => (testEmailSending.value = false)
    )
    ElMessage.success('发送成功')
  }
  watchEffect(() => {
    settings.value = [...items]
  })
</script>

<style scoped lang="scss"></style>
