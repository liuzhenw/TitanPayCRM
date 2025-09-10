<template>
  <div class="process-actions">
    <!-- 待处理状态 -->
    <div v-if="withdrawalRequest.status === 'pending'" class="status-section pending">
      <div class="status-icon">
        <el-icon><Clock /></el-icon>
      </div>
      <div class="status-content">
        <h3>申请待处理</h3>
        <p>请仔细核对申请信息后，选择批准或拒绝此申请。</p>
        <div class="quick-info">
          <div class="info-item">
            <span class="label">申请人：</span>
            <span class="value">{{ withdrawalRequest.referrer.email }}</span>
          </div>
          <div class="info-item">
            <span class="label">申请金额：</span>
            <span class="value amount">{{ withdrawalRequest.amount }} USDT</span>
          </div>
          <div class="info-item">
            <span class="label">提现地址：</span>
            <span class="value"><EllipticalLabel :value="withdrawalRequest.toAddress" /></span>
          </div>
          <div class="info-item">
            <span class="label">申请时间：</span>
            <span class="value"><Datetime :value="withdrawalRequest.createdAt" /></span>
          </div>
        </div>
      </div>
    </div>

    <!-- 已批准状态 -->
    <div v-else-if="withdrawalRequest.status === 'approved'" class="status-section approved">
      <div class="status-icon">
        <el-icon><CircleCheck /></el-icon>
      </div>
      <div class="status-content">
        <h3>申请已批准</h3>
        <p>此提现申请已经批准并完成。</p>
        <div class="result-details">
          <div class="detail-item">
            <span class="label">交易哈希：</span>
            <span class="value"><EllipticalLabel :value="withdrawalRequest.txid" /></span>
          </div>
          <div class="detail-item">
            <span class="label">完成时间：</span>
            <span class="value"><Datetime :value="withdrawalRequest.completedAt" /></span>
          </div>
        </div>
      </div>
    </div>

    <!-- 已拒绝状态 -->
    <div v-else-if="withdrawalRequest.status === 'rejected'" class="status-section rejected">
      <div class="status-icon">
        <el-icon><CircleClose /></el-icon>
      </div>
      <div class="status-content">
        <h3>申请已拒绝</h3>
        <p>此提现申请已被拒绝。</p>
        <div class="reject-reason">
          <div class="reason-item">
            <span class="label">拒绝原因：</span>
            <span class="value">{{ withdrawalRequest.rejectReason }}</span>
          </div>
          <div class="reason-item">
            <span class="label">处理时间：</span>
            <span class="value"><Datetime :value="withdrawalRequest.completedAt" /></span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { WithdrawalRequestDto } from '@/api/services'
  import { Clock, CircleCheck, CircleClose } from '@element-plus/icons-vue'

  defineProps<{
    withdrawalRequest: WithdrawalRequestDto
  }>()
</script>

<style scoped lang="scss">
  .process-actions {
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

          .info-item,
          .detail-item,
          .reason-item {
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

              &.amount {
                font-weight: 600;
                color: #E6A23C;
              }
            }
          }
        }
      }

      &.pending {
        background: linear-gradient(135deg, #FFF7E6 0%, #FFF3CD 100%);
        border: 1px solid #FFE4B5;

        .status-icon {
          background: #FAAD14;
          color: white;
        }

        h3 {
          color: #D48806;
        }
      }

      &.approved {
        background: linear-gradient(135deg, #F0F9FF 0%, #E6F7FF 100%);
        border: 1px solid #B5F5EC;

        .status-icon {
          background: #52C41A;
          color: white;
        }

        h3 {
          color: #389E0D;
        }
      }

      &.rejected {
        background: linear-gradient(135deg, #FFF2F0 0%, #FFE6E6 100%);
        border: 1px solid #FFCCC7;

        .status-icon {
          background: #FF4D4F;
          color: white;
        }

        h3 {
          color: #CF1322;
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
  }
</style>
