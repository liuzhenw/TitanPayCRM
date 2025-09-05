<template>
  <el-drawer v-model="modelValue">
    <template #header>
      <span>修改 {{ value.user.email }} </span>
    </template>

    <el-form ref="form" :model="model" :rules="rules" label-width="auto">
      <el-form-item label="邮箱地址" prop="emial">
        <span>{{ value.user.email }}</span>
      </el-form-item>
      <el-form-item label="推广等级" prop="levelId">
        <el-select
          v-model="model.levelId"
          placeholder="请选择推广等级"
          clearable
          style="width: 100%"
        >
          <el-option
            v-for="level in levels"
            :key="level.value"
            :label="level.label"
            :value="level.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="累计推广">
        <span>{{ value.totalCount }}</span>
      </el-form-item>
      <el-form-item label="累计佣金">
        <span>{{ value.withdrawal + value.commission }} USD</span>
      </el-form-item>
      <el-form-item label="直推人数">
        <span>{{ value.directCount }}</span>
      </el-form-item>
      <el-form-item label="间推人数">
        <span>{{ value.indirectCount }}</span>
      </el-form-item>
      <el-form-item label="可用佣金">
        <span>{{ value.commission }} USD</span>
      </el-form-item>
      <el-form-item label="累计提款">
        <span>{{ value.withdrawal }} USD</span>
      </el-form-item>
      <el-form-item label="提款地址">
        <EllipticalLabel :value="value.withdrawalAddress" />
      </el-form-item>
      <el-form-item label="加入黑名单" prop="isDisabled">
        <el-switch v-model="model.isDisabled" style="--el-switch-on-color: #ff4949"/>
      </el-form-item>
      <el-form-item label="备注说明" prop="remark">
        <el-input
          v-model="model.remark"
          type="textarea"
          :rows="4"
          placeholder="请输入备注信息"
          maxlength="255"
          show-word-limit
        />
      </el-form-item>
    </el-form>

    <template #footer>
      <el-button type="primary" :loading="isSubmitting" @click="onSubmit">提交</el-button>
      <el-button @click="onCancel">取消</el-button>
    </template>
  </el-drawer>
</template>

<script setup lang="ts">
  import {
    ReferrerService,
    ReferrerDto,
    ReferrerUpdateInput,
    ReferralLevelService
  } from '@/api/services'
  import { ElMessage, FormInstance, FormRules } from 'element-plus'
  defineOptions({ name: 'ReferrerUpdateDrawer' })
  const emit = defineEmits(['submitted'])
  const modelValue = defineModel({ type: Boolean })
  const { value } = defineProps<{ value: ReferrerDto }>()
  const form = ref<FormInstance>()
  const levels = ref<{ label: string; value: string }[]>([])
  const model = reactive<ReferrerUpdateInput>({
    levelId: value.level?.id,
    isDisabled: value.isDisabled,
    remark: value.remark
  })
  const rules: FormRules = {}
  watch(
    () => value,
    () => {
      model.isDisabled = value.isDisabled
      model.levelId = value.level?.id
      model.remark = value.remark
    }
  )
  const onCancel = () => {
    modelValue.value = false
  }
  const isSubmitting = ref(false)
  const onSubmit = async () => {
    const valid = await form.value?.validate()
    if (!valid) return

    isSubmitting.value = true
    const res = await ReferrerService.update(value.id, model).finally(
      () => (isSubmitting.value = false)
    )
    ElMessage.success('修改成功')
    modelValue.value = false
    emit('submitted')
  }

  onMounted(async () => {
    levels.value = await ReferralLevelService.getOptions()
  })
</script>

<style scoped lang="scss"></style>
