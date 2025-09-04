<template>
  <el-dialog
    v-model="modelValue"
    title="修改登录密码"
    width="400"
    append-to-body
    @closed="onClosed"
  >
    <el-form ref="form" :model="model" :rules="rules">
      <el-form-item label="旧的密码" prop="oldPassword">
        <el-input v-model="model.oldPassword" type="password" show-password />
      </el-form-item>
      <el-form-item label="新的密码" prop="newPassword">
        <el-input v-model="model.newPassword" type="password" show-password />
      </el-form-item>
      <el-form-item label="确认密码" prop="confirmPassword">
        <el-input v-model="model.confirmPassword" type="password" show-password />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="() => (modelValue = false)">取消</el-button>
      <el-button type="primary" :loading="submitting" @click="onSubmit">修改</el-button>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
  import { useUserStore } from '@/store/modules/user'
  import { AccountService, ChangePasswordInput } from '@/api/accountApi'
  import { FormInstance, FormRules } from 'element-plus'
  const modelValue = defineModel({ type: Boolean })
  const form = ref<FormInstance>()
  const model = reactive<ChangePasswordInput>({
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  })
  const rules: FormRules = {
    oldPassword: [
      { required: true, message: '请输入旧密码', trigger: 'blur' },
      { min: 6, max: 32, message: '长度在 6-32 位之间', trigger: 'blur' }
    ],
    newPassword: [
      { required: true, message: '请输入新密码', trigger: 'blur' },
      { min: 6, max: 32, message: '长度在 6-32 位之间', trigger: 'blur' },
      {
        validator(_, value, callback) {
          if (model.oldPassword.length > 0 && value === model.oldPassword) {
            callback(new Error('新密码不能与旧密码相同'))
          } else {
            callback()
          }
        }
      }
    ],
    confirmPassword: [
      { required: true, message: '请输入确认密码', trigger: 'blur' },
      {
        validator(_, value, callback) {
          if (value !== model.newPassword) {
            callback(new Error('两次输入的密码不一致'))
          } else {
            callback()
          }
        }
      }
    ]
  }

  const onClosed = () => {
    model.oldPassword = ''
    model.newPassword = ''
    model.confirmPassword = ''
  }
  const userStore = useUserStore()
  const submitting = ref(false)
  const onSubmit = async () => {
    const valid = await form.value?.validate()
    if (!valid) return

    submitting.value = true
    await AccountService.changePassword(model).finally(() => (submitting.value = false))
    userStore.logOut()
  }
</script>

<style scoped lang="scss"></style>
