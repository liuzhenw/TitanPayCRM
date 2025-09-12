<template>
  <div :class="['status-section', statusClass]">
    <div class="status-icon">
      <el-icon><component :is="iconComponent" /></el-icon>
    </div>
    <div class="status-content">
      <h3>{{ title }}</h3>
      <p>{{ description }}</p>
      <div :class="contentClass">
        <div v-for="(item, index) in items" :key="index" class="info-item">
          <span class="label">{{ item.label }}：</span>
          <span :class="['value', item.valueClass]">
            <component v-if="item.component" :is="item.component" v-bind="item.componentProps">
              {{ item.value }}
            </component>
            <template v-else>
              {{ item.value }}
            </template>
          </span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { computed } from 'vue'
  import { Clock, CircleCheck, CircleClose, Lock } from '@element-plus/icons-vue'
  import {
    ReferrerRequestDto,
    ReferrerDto,
    ReferrerService,
    ReferrerWithDetailsDto
  } from '@/api/services'

  interface InfoItem {
    label: string
    value: string
    valueClass?: string
    component?: any
    componentProps?: any
  }

  interface Props {
    request: ReferrerRequestDto
  }

  const props = defineProps<Props>()

  const referrer = ref<ReferrerWithDetailsDto>()
  const statusClass = computed(() => props.request.status)
  const title = computed(() => {
    switch (props.request.status) {
      case 'pending':
        return '申请待处理'
      case 'approved':
        return '申请已批准'
      case 'rejected':
        return '申请已拒绝'
      case 'disabled':
        return '已禁止此用户申请'
    }
  })
  const description = computed(() => {
    switch (props.request.status) {
      case 'pending':
        return '请仔细核对申请信息后，选择批准或拒绝此申请。'
      case 'approved':
        return '此代理申请已经批准并完成。'
      case 'rejected':
        return '此申请已被拒绝。'
      case 'disabled':
        return '此用户的代理申请已被拒绝且用户已被禁用。'
    }
  })
  const contentClass = computed(() => {
    switch (props.request.status) {
      case 'pending':
        return 'quick-info'
      case 'approved':
        return 'result-details'
      case 'rejected':
      case 'disabled':
        return 'reject-reason'
      default:
        return undefined
    }
  })

  const iconComponent = computed(() => {
    switch (props.request.status) {
      case 'pending':
        return Clock
      case 'approved':
        return CircleCheck
      case 'rejected':
        return CircleClose
      case 'disabled':
        return Lock
      default:
        return Clock
    }
  })

  const items = computed<InfoItem[]>(() => {
    let items = []

    // 添加referrer信息
    if (referrer.value) {
      items.push(
        {
          label: '直推人数',
          value: referrer.value.directCount.toString()
        },
        {
          label: '间推人数',
          value: referrer.value.indirectCount.toString()
        }
      )
      if (referrer.value.remark) {
        items.push({
          label: '备注信息',
          value: referrer.value.remark
        })
      }
    }

    if (props.request.status === 'rejected' || props.request.status === 'disabled') {
      items.push({
        label: '拒绝原因',
        value: props.request.rejectReason || ''
      })
    }
    return items
  })

  const initialize = async () => {
    referrer.value = await ReferrerService.get(props.request.id)
  }
  watchEffect(async () => {
    await initialize()
  })
</script>

<style scoped lang="scss">
  .status-section {
    display: flex;
    align-items: flex-start;
    gap: 16px;
    padding: 20px;
    border-radius: 8px;
    transition: all 0.3s ease;

    .status-icon {
      flex-shrink: 0;
      width: 48px;
      height: 48px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;

      .el-icon {
        font-size: 24px;
        color: white;
      }
    }

    .status-content {
      flex: 1;

      h3 {
        margin: 0 0 8px 0;
        font-size: 18px;
        font-weight: 600;
      }

      p {
        margin: 0 0 16px 0;
        color: #666;
        font-size: 14px;
        line-height: 1.5;
      }

      .quick-info,
      .result-details,
      .reject-reason {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
        gap: 12px;

        .info-item {
          display: flex;
          align-items: center;
          gap: 8px;

          .label {
            font-weight: 500;
            color: #666;
            min-width: 80px;
          }

          .value {
            color: #333;
            flex: 1;

            &.level {
              font-weight: 600;
              color: #409eff;
            }
          }
        }
      }
    }

    &.pending {
      background: linear-gradient(135deg, #fff7e6 0%, #fff3cd 100%);
      border: 1px solid #ffe4b5;

      .status-icon {
        background: #faad14;
      }

      h3 {
        color: #d48806;
      }
    }

    &.approved {
      background: linear-gradient(135deg, #f0f9ff 0%, #e6f7ff 100%);
      border: 1px solid #b5f5ec;

      .status-icon {
        background: #52c41a;
      }

      h3 {
        color: #389e0d;
      }
    }

    &.rejected {
      background: linear-gradient(135deg, #fff2f0 0%, #ffe6e6 100%);
      border: 1px solid #ffccc7;

      .status-icon {
        background: #ff4d4f;
      }

      h3 {
        color: #cf1322;
      }
    }

    &.disabled {
      background: linear-gradient(135deg, #f5f5f5 0%, #e8e8e8 100%);
      border: 1px solid #d9d9d9;

      .status-icon {
        background: #8c8c8c;
      }

      h3 {
        color: #595959;
      }
    }

    &:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    }
  }

  // 响应式设计
  @media (max-width: 768px) {
    .status-section {
      flex-direction: column;
      text-align: center;

      .status-icon {
        margin: 0 auto;
      }

      .quick-info,
      .result-details,
      .reject-reason {
        grid-template-columns: 1fr;
      }
    }
  }
</style>
