<template>
  <FullScreen>
    <div id="container">
      <el-page-header @back="goBack">
        <template #content>
          <span class="text-large font-600 mr-3"> 代理申请详情 </span>
        </template>
      </el-page-header>

      <div v-loading="!referrerRequest" class="content">
        <div v-if="referrerRequest" class="details-container">
          <!-- 申请信息卡片 -->
          <el-card shadow="never" class="info-card">
            <template #header>
              <div class="card-header">
                <span class="card-title">
                  <el-icon><Document /></el-icon>
                  申请信息
                </span>
                <StatusTag :value="referrerRequest.status" />
              </div>
            </template>
            <BasicInfo :referrerRequest="referrerRequest" />
          </el-card>

          <!-- 申请处理卡片 -->
          <el-card shadow="never" class="process-card">
            <template #header>
              <div class="card-header">
                <span class="card-title">
                  <el-icon><Setting /></el-icon>
                  申请处理
                </span>
                <div class="header-actions" v-if="referrerRequest?.status === 'pending'">
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
              :referrerRequest="referrerRequest"
              @approve="handleApprove"
              @reject="handleReject"
            />
          </el-card>
        </div>
      </div>
    </div>

    <!-- Approve Dialog -->
    <el-dialog
      v-model="approveDialog.visible"
      title="批准代理申请"
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
          <span>{{ approveDialog.form.userName }}</span>
        </el-form-item>
        <el-form-item label="申请等级">
          <span>{{ approveDialog.form.levelName }}</span>
        </el-form-item>
        <el-form-item label="申请时间">
          <span>{{ approveDialog.form.createdAt }}</span>
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
      title="拒绝代理申请"
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
          <span>{{ rejectDialog.form.userName }}</span>
        </el-form-item>
        <el-form-item label="申请等级">
          <span>{{ rejectDialog.form.levelName }}</span>
        </el-form-item>
        <el-form-item label="拒绝原因" prop="reason">
          <el-input
            v-model="rejectDialog.form.reason"
            placeholder="请输入拒绝原因"
            type="textarea"
            :rows="3"
          />
        </el-form-item>
        <el-form-item label="是否禁用" prop="isDisabled">
          <el-switch
            v-model="rejectDialog.form.isDisabled"
            style="--el-switch-on-color: #ff4949"
            active-text="禁止此用户申请"
            inactive-text="仅拒绝申请"
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
  </FullScreen>
</template>

<script setup lang="ts">
  import { ref, reactive, onMounted } from 'vue'
  import { useRouter, useRoute } from 'vue-router'
  import { ElMessage, type FormInstance, type FormRules } from 'element-plus'
  import { ReferrerRequestService, ReferrerRequestDto } from '@/api/services'
  import BasicInfo from './basic-info.vue'
  import ProcessActions from './process-actions.vue'
  import { Check, Close, Document, Setting } from '@element-plus/icons-vue'
  import StatusTag from '../statusTag.vue'

  const router = useRouter()
  const route = useRoute()
  const referrerRequestId = route.params.id as string
  const referrerRequest = ref<ReferrerRequestDto>()

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
      userName: '',
      userEmail: '',
      levelName: '',
      createdAt: ''
    },
    rules: {} as FormRules
  })

  // Reject Dialog
  const rejectFormRef = ref<FormInstance>()
  const rejectDialog = reactive({
    visible: false,
    loading: false,
    form: {
      id: '',
      userName: '',
      userEmail: '',
      levelName: '',
      reason: '',
      isDisabled: false
    },
    rules: {
      reason: [
        { required: true, message: '请输入拒绝原因', trigger: 'blur' },
        { min: 1, max: 255, message: '拒绝原因长度在 1 到 255 个字符', trigger: 'blur' }
      ]
    } as FormRules
  })

  const onApprove = () => {
    if (!referrerRequest.value) return

    approveDialog.form.id = referrerRequest.value.id
    approveDialog.form.userName = referrerRequest.value.user.name
    approveDialog.form.userEmail = referrerRequest.value.user.email
    approveDialog.form.levelName = referrerRequest.value.level.name
    approveDialog.form.createdAt = referrerRequest.value.createdAt
    approveDialog.visible = true
  }

  const onReject = () => {
    if (!referrerRequest.value) return

    rejectDialog.form.id = referrerRequest.value.id
    rejectDialog.form.userName = referrerRequest.value.user.name
    rejectDialog.form.userEmail = referrerRequest.value.user.email
    rejectDialog.form.levelName = referrerRequest.value.level.name
    rejectDialog.form.reason = ''
    rejectDialog.form.isDisabled = false
    rejectDialog.visible = true
  }

  const handleApproveSubmit = async () => {
    try {
      approveDialog.loading = true

      await ReferrerRequestService.approve(approveDialog.form.id)

      ElMessage.success('代理申请已批准')
      approveDialog.visible = false
      // 刷新数据
      await loadReferrerRequest()
    } catch (error) {
      console.error('Approve referrer request failed:', error)
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

      await ReferrerRequestService.reject(rejectDialog.form.id, {
        reason: rejectDialog.form.reason,
        isDisabled: rejectDialog.form.isDisabled
      })

      ElMessage.success('代理申请已拒绝')
      rejectDialog.visible = false
      // 刷新数据
      await loadReferrerRequest()
    } catch (error) {
      console.error('Reject referrer request failed:', error)
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

  const loadReferrerRequest = async () => {
    try {
      // Note: Backend doesn't have a Get method, so we'll get it from the list
      const result = await ReferrerRequestService.getList({ id: referrerRequestId })
      if (result.items.length > 0) {
        referrerRequest.value = result.items[0]
      } else {
        ElMessage.error('找不到该代理申请')
        router.back()
      }
    } catch (error) {
      ElMessage.error('获取代理申请详情失败')
      router.back()
    }
  }

  onMounted(async () => {
    await loadReferrerRequest()
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
