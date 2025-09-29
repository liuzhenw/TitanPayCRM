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
        <div class="commission-field">
          <span>{{ value.commission }} USD</span>
          <el-button
              type="primary"
              icon="Edit"
              link
              @click="showCommissionDialog = true"
          >
            修改
          </el-button>
        </div>
      </el-form-item>
      <el-form-item label="累计提款">
        <span>{{ value.withdrawal }} USD</span>
      </el-form-item>
      <el-form-item label="提款地址">
        <EllipticalLabel :value="value.withdrawalAddress"/>
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

    <!-- 修改佣金对话框 -->
    <el-dialog
        v-model="showCommissionDialog"
        title="修改佣金"
        width="400px"
        :close-on-click-modal="false"
    >
      <el-form
          ref="commissionForm"
          :model="commissionModel"
          :rules="commissionRules"
          label-width="auto"
      >
        <el-form-item label="当前佣金">
          <span>{{ value.commission }} USD</span>
        </el-form-item>
        <el-form-item label="佣金变化金额" prop="commission">
          <el-input-number
              v-model="commissionModel.commission"
              :precision="2"
              :step="1"
              style="width: 100%"
              placeholder="请输入佣金变化金额（正数增加，负数减少）"
              controls-position="right"
          />
          <div class="commission-preview">
            <span>修改后佣金: {{ (value.commission + commissionModel.commission).toFixed(2) }} USD</span>
          </div>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button
            type="primary"
            :loading="isCommissionSubmitting"
            @click="onCommissionSubmit"
        >
          确认修改
        </el-button>
        <el-button @click="showCommissionDialog = false">取消</el-button>
      </template>
    </el-dialog>
  </el-drawer>
</template>

<script setup lang="ts">
import {
  ReferrerService,
  ReferrerDto,
  ReferrerUpdateInput,
  ReferralLevelService,
  ReferrerChangeCommissionInput
} from '@/api/services'
import {ElMessage, FormInstance, FormRules} from 'element-plus'

defineOptions({name: 'ReferrerUpdateDrawer'})
const emit = defineEmits(['submitted'])
const modelValue = defineModel({type: Boolean})
const {value} = defineProps<{ value: ReferrerDto }>()
const form = ref<FormInstance>()
const levels = ref<{ label: string; value: string }[]>([])
const model = reactive<ReferrerUpdateInput>({
  levelId: value.level?.id,
  isDisabled: value.isDisabled,
  remark: value.remark
})
const rules: FormRules = {}

// 修改佣金相关
const showCommissionDialog = ref(false)
const commissionForm = ref<FormInstance>()
const commissionModel = reactive<ReferrerChangeCommissionInput>({
  commission: 0
})
const isCommissionSubmitting = ref(false)
const commissionRules: FormRules = {
  commission: [
    {required: true, message: '请输入佣金变化金额', trigger: 'blur'},
    {type: 'number', message: '请输入有效的数字', trigger: 'blur'},
    {
      validator: (_, inputValue, callback) => {
        if (inputValue < 0 && Math.abs(inputValue) > value.commission) {
          callback(new Error('减少的佣金不能超过当前可用佣金'))
        } else {
          callback()
        }
      },
      trigger: 'blur'
    }
  ]
}
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
  await ReferrerService.update(value.id, model).finally(
      () => (isSubmitting.value = false)
  )
  ElMessage.success('修改成功')
  modelValue.value = false
  emit('submitted')
}

// 修改佣金方法
const onCommissionSubmit = async () => {
  const valid = await commissionForm.value?.validate()
  if (!valid) return

  isCommissionSubmitting.value = true
  try {
    // 直接调用 API，传入变化金额
    await ReferrerService.changeCommission(value.id, commissionModel)

    ElMessage.success('佣金修改成功')
    showCommissionDialog.value = false
    modelValue.value = false
    emit('submitted')
  } catch (error) {
    console.error('修改佣金失败:', error)
  } finally {
    isCommissionSubmitting.value = false
  }
}

// 重置佣金表单
watch(showCommissionDialog, (newVal) => {
  if (newVal) {
    commissionModel.commission = 0 // 默认变化金额为0
    commissionForm.value?.clearValidate()
  }
})

onMounted(async () => {
  levels.value = await ReferralLevelService.getOptions()
  await ReferrerService.get(value.id)
})
</script>

<style scoped lang="scss">
.commission-field {
  display: flex;
  align-items: center;
  gap: 12px;

  span {
    font-weight: 500;
  }
}

.commission-preview {
  margin-top: 8px;
  padding: 8px;
  background-color: var(--el-fill-color-light);
  border-radius: 4px;
  font-size: 13px;

  span {
    display: block;
    margin-bottom: 4px;

    &:last-child {
      margin-bottom: 0;
      font-weight: 600;
      color: var(--el-color-primary);
    }
  }
}
</style>
