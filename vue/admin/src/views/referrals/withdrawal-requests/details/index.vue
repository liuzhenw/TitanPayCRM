<template>
  <div id="container">
    <el-page-header @back="goBack">
      <template #content>
        <span class="text-large font-600 mr-3"> 提现申请详情 </span>
      </template>
    </el-page-header>

    <div v-loading="!withdrawalRequest" class="content">
      <div v-if="withdrawalRequest" class="details-container">
        <!-- 申请信息卡片 -->
        <el-card shadow="never" class="info-card">
          <template #header>
            <div class="card-header">
              <span class="card-title">
                <el-icon><Document /></el-icon>
                申请信息
              </span>
              <StatusTag :value="withdrawalRequest.status" />
            </div>
          </template>
          <BasicInfo :withdrawalRequest="withdrawalRequest" />
        </el-card>

        <!-- 申请处理卡片 -->
        <el-card shadow="never" class="process-card">
          <template #header>
            <div class="card-header">
              <span class="card-title">
                <el-icon><Setting /></el-icon>
                申请处理
              </span>
              <div class="header-actions" v-if="withdrawalRequest?.status === 'pending'">
                <el-button type="danger" @click="onReject" size="large">
                  <el-icon><Close /></el-icon>
                  拒绝申请
                </el-button>
                <el-button type="success" @click="onApprove" size="large">
                  <el-icon><Check /></el-icon>
                  批准申请
                </el-button>
              </div>
            </div>
          </template>
          <ProcessActions
            :withdrawalRequest="withdrawalRequest"
            @approve="handleApprove"
            @reject="handleReject"
          />
        </el-card>
      </div>
    </div>

    <!-- Approve Dialog -->
    <el-dialog
      v-model="approveDialog.visible"
      title="批准提现申请"
      width="500px"
      :close-on-click-modal="false"
    >
      <el-form
        ref="approveFormRef"
        :model="approveDialog.form"
        :rules="approveDialog.rules"
        label-width="100px"
      >
        <el-form-item label="申请人">
          <span>{{ approveDialog.form.referrerEmail }}</span>
        </el-form-item>
        <el-form-item label="提现金额">
          <span>{{ approveDialog.form.amount }} USDT</span>
        </el-form-item>
        <el-form-item label="提现地址">
          <el-space>
            <span>{{ approveDialog.form.toAddress }}</span>
            <QrcodeIcon :value="approveDialog.form.toAddress"/>
          </el-space>
        </el-form-item>
        <el-form-item label="交易哈希" prop="txid">
          <el-input
            v-model="approveDialog.form.txid"
            placeholder="请输入交易哈希"
            type="textarea"
            :rows="3"
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="approveDialog.visible = false">取消</el-button>
          <el-button type="primary" @click="handleApproveSubmit" :loading="approveDialog.loading">
            确认批准
          </el-button>
        </span>
      </template>
    </el-dialog>

    <!-- Reject Dialog -->
    <el-dialog
      v-model="rejectDialog.visible"
      title="拒绝提现申请"
      width="500px"
      :close-on-click-modal="false"
    >
      <el-form
        ref="rejectFormRef"
        :model="rejectDialog.form"
        :rules="rejectDialog.rules"
        label-width="100px"
      >
        <el-form-item label="申请人">
          <span>{{ rejectDialog.form.referrerEmail }}</span>
        </el-form-item>
        <el-form-item label="提现金额">
          <span>{{ rejectDialog.form.amount }} USDT</span>
        </el-form-item>
        <el-form-item label="提现地址">
          <span>{{ rejectDialog.form.toAddress }}</span>
        </el-form-item>
        <el-form-item label="拒绝原因" prop="reason">
          <el-input
            v-model="rejectDialog.form.reason"
            placeholder="请输入拒绝原因"
            type="textarea"
            :rows="3"
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="rejectDialog.visible = false">取消</el-button>
          <el-button type="danger" @click="handleRejectSubmit" :loading="rejectDialog.loading">
            确认拒绝
          </el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
  import { ref, reactive, onMounted } from 'vue'
  import { useRouter, useRoute } from 'vue-router'
  import { ElMessage, type FormInstance, type FormRules } from 'element-plus'
  import { WithdrawalRequestService, WithdrawalRequestDto } from '@/api/services'
  import BasicInfo from './basic-info.vue'
  import ProcessActions from './process-actions.vue'
  import { Check, Close, Document, Setting } from '@element-plus/icons-vue'
  import statusTag from '../statusTag.vue'
  import StatusTag from '../statusTag.vue'

  const router = useRouter()
  const route = useRoute()
  const withdrawalRequestId = route.params.id as string
  const withdrawalRequest = ref<WithdrawalRequestDto>()

  const goBack = () => {
    router.back()
  }

  // Approve Dialog
  const approveFormRef = ref<FormInstance>()
  const approveDialog = reactive({
    visible: false,
    loading: false,
    form: {
      id: '',
      referrerEmail: '',
      amount: 0,
      toAddress: '',
      txid: ''
    },
    rules: {
      txid: [
        { required: true, message: '请输入交易哈希', trigger: 'blur' },
        { min: 1, max: 128, message: '交易哈希长度在 1 到 128 个字符', trigger: 'blur' }
      ]
    } as FormRules
  })

  // Reject Dialog
  const rejectFormRef = ref<FormInstance>()
  const rejectDialog = reactive({
    visible: false,
    loading: false,
    form: {
      id: '',
      referrerEmail: '',
      amount: 0,
      toAddress: '',
      reason: ''
    },
    rules: {
      reason: [
        { required: true, message: '请输入拒绝原因', trigger: 'blur' },
        { min: 1, max: 255, message: '拒绝原因长度在 1 到 255 个字符', trigger: 'blur' }
      ]
    } as FormRules
  })

  const onApprove = () => {
    if (!withdrawalRequest.value) return

    approveDialog.form.id = withdrawalRequest.value.id
    approveDialog.form.referrerEmail = withdrawalRequest.value.referrer.email
    approveDialog.form.amount = withdrawalRequest.value.amount
    approveDialog.form.toAddress = withdrawalRequest.value.toAddress
    approveDialog.form.txid = ''
    approveDialog.visible = true
  }

  const onReject = () => {
    if (!withdrawalRequest.value) return

    rejectDialog.form.id = withdrawalRequest.value.id
    rejectDialog.form.referrerEmail = withdrawalRequest.value.referrer.email
    rejectDialog.form.amount = withdrawalRequest.value.amount
    rejectDialog.form.toAddress = withdrawalRequest.value.toAddress
    rejectDialog.form.reason = ''
    rejectDialog.visible = true
  }

  const handleApproveSubmit = async () => {
    if (!approveFormRef.value) return

    try {
      await approveFormRef.value.validate()
      approveDialog.loading = true

      await WithdrawalRequestService.approve(approveDialog.form.id, {
        txid: approveDialog.form.txid
      })

      ElMessage.success('提现申请已批准')
      approveDialog.visible = false
      // 刷新数据
      await loadWithdrawalRequest()
    } catch (error) {
      console.error('Approve withdrawal request failed:', error)
      ElMessage.error('批准失败，请重试')
    } finally {
      approveDialog.loading = false
    }
  }

  const handleRejectSubmit = async () => {
    if (!rejectFormRef.value) return

    try {
      await rejectFormRef.value.validate()
      rejectDialog.loading = true

      await WithdrawalRequestService.reject(rejectDialog.form.id, {
        reason: rejectDialog.form.reason
      })

      ElMessage.success('提现申请已拒绝')
      rejectDialog.visible = false
      // 刷新数据
      await loadWithdrawalRequest()
    } catch (error) {
      console.error('Reject withdrawal request failed:', error)
      ElMessage.error('拒绝失败，请重试')
    } finally {
      rejectDialog.loading = false
    }
  }

  const handleApprove = () => {
    onApprove()
  }

  const handleReject = () => {
    onReject()
  }
  const loadWithdrawalRequest = async () => {
    try {
      withdrawalRequest.value = await WithdrawalRequestService.get(withdrawalRequestId)
    } catch (error) {
      ElMessage.error('获取提现申请详情失败')
      router.back()
    }
  }

  onMounted(async () => {
    await loadWithdrawalRequest()
  })
</script>

<style scoped lang="scss">
  .content {
    margin-top: 20px;
    min-height: 400px;
  }

  .details-container {
    display: flex;
    flex-direction: column;
    gap: 16px;
  }

  .info-card,
  .process-card {
    border-radius: 8px;
    transition: all 0.3s ease;

    &:hover {
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    }
  }

  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;

    .card-title {
      display: flex;
      align-items: center;
      gap: 8px;
      font-size: 16px;
      font-weight: 600;
      color: #333;

      .el-icon {
        font-size: 18px;
        color: #409eff;
      }
    }

    .header-actions {
      display: flex;
      gap: 8px;

      .el-button {
        display: flex;
        align-items: center;
        gap: 4px;
      }
    }
  }

  // 响应式设计
  @media (max-width: 768px) {
    .card-header {
      flex-direction: column;
      align-items: flex-start;
      gap: 12px;

      .header-actions {
        width: 100%;
        justify-content: flex-end;
      }
    }
  }
</style>
