<template>
  <div id="container">
    <el-page-header @back="goBack">
      <template #content>
        <span class="text-large font-600 mr-3"> 推荐人详情 </span>
      </template>
    </el-page-header>

    <div v-loading="!referrer" class="content">
      <el-tabs v-if="referrer" v-model="activeTab" class="referrer-tabs">
        <!-- 推荐人详情 Tab -->
        <el-tab-pane name="details" label="基础信息">
          <div class="details-content">
            <el-row :gutter="16" class="equal-height-row">
              <el-col :md="8" :sm="24" class="equal-height-col">
                <el-card shadow="never" class="panel" style="height: 100%">
                  <template #header>
                    <span class="text-large font-600"> 基本信息 </span>
                  </template>
                  <Basic :referrer="referrer" />
                </el-card>
              </el-col>
              <el-col :md="16" :sm="24" class="equal-height-col">
                <el-row :gutter="16">
                  <el-col :span="24">
                    <el-card shadow="never" class="panel">
                      <template #header>
                        <span class="text-large font-600"> 推荐统计 </span>
                      </template>
                      <Statistics :referrer="referrer" />
                    </el-card>
                  </el-col>

                  <el-col :span="24">
                    <el-card shadow="never" class="panel">
                      <template #header>
                        <span class="text-large font-600"> 邀请关系 </span>
                      </template>
                      <el-tabs>
                        <el-tab-pane label="下级用户">
                          <Recommendees :referrer="referrer" />
                        </el-tab-pane>
                        <el-tab-pane label="上级用户">
                          <Ancestors :referrer="referrer" />
                        </el-tab-pane>
                      </el-tabs>
                    </el-card>
                  </el-col>
                </el-row>
              </el-col>
            </el-row>
          </div>
        </el-tab-pane>

        <!-- 佣金记录 Tab -->
        <el-tab-pane name="commission" label="佣金记录" lazy>
          <CommissionLogs :referrer-id="referrerId" />
        </el-tab-pane>

        <!-- 提款申请 Tab -->
        <el-tab-pane name="withdrawal" label="提款申请" lazy>
          <WithdrawalRequests :referrer-id="referrerId" />
        </el-tab-pane>
      </el-tabs>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRouter, useRoute } from 'vue-router'
  import { ElMessage } from 'element-plus'
  import { ReferrerService, ReferrerWithDetailsDto } from '@/api/services'
  import Basic from './basic.vue'
  import Statistics from './statistics.vue'
  import Recommendees from './recommendees.vue'
  import Ancestors from './ancestors.vue'
  import CommissionLogs from './commission-logs.vue'
  import WithdrawalRequests from './withdrawal-requests.vue'

  const router = useRouter()
  const route = useRoute()
  const referrerId = route.params.id as string
  const referrer = ref<ReferrerWithDetailsDto>()

  // Tab控制
  const activeTab = ref('details')

  const goBack = () => {
    router.back()
  }

  onMounted(async () => {
    try {
      referrer.value = await ReferrerService.get(referrerId)
    } catch {
      ElMessage.error('获取推荐人详情失败')
      router.back()
    }
  })
</script>

<style scoped lang="scss">
  .content {
    margin-top: 20px;
  }

  .main-card {
    border-radius: 8px;
    overflow: hidden;

    .referrer-tabs {
      :deep(.el-tabs__header) {
        margin: 0;
        background: #f8f9fa;
        border-bottom: 1px solid #e9ecef;
      }

      :deep(.el-tabs__nav-wrap) {
        padding: 0 20px;
      }

      :deep(.el-tabs__item) {
        font-size: 14px;
        font-weight: 500;
        color: #666;
        padding: 0 20px;
        height: 50px;
        line-height: 50px;

        &.is-active {
          color: #409eff;
          font-weight: 600;
        }

        &:hover {
          color: #409eff;
        }
      }

      :deep(.el-tabs__active-bar) {
        background: #409eff;
      }

      :deep(.el-tabs__content) {
        padding: 20px;
      }
    }
  }

  .details-content {
    .panel {
      margin-bottom: 16px;
      transition: all 0.3s ease;

      &:hover {
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
      }
    }
  }

  .tab-content {
    .tab-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
      padding-bottom: 16px;
      border-bottom: 1px solid #e9ecef;

      .el-button {
        display: flex;
        align-items: center;
        gap: 4px;
      }
    }
  }

  .equal-height-row {
    display: flex;
    flex-wrap: wrap;
  }

  .equal-height-col {
    display: flex;
    flex-direction: column;
  }

  // 响应式设计
  @media (max-width: 768px) {
    .main-card {
      .referrer-tabs {
        :deep(.el-tabs__nav-wrap) {
          padding: 0 10px;
        }

        :deep(.el-tabs__item) {
          padding: 0 12px;
          font-size: 13px;
        }

        :deep(.el-tabs__content) {
          padding: 12px;
        }
      }
    }

    .tab-header {
      flex-direction: column;
      align-items: flex-start;
      gap: 8px;

      .el-button {
        align-self: flex-end;
      }
    }
  }
</style>
