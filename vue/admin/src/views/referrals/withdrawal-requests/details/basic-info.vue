<template>
  <div class="basic-info">
    <div class="info-grid">
      <div class="info-item">
        <div class="item-icon">
          <el-icon><User /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">申请人</div>
          <div class="item-value">{{ withdrawalRequest.referrer.email }}</div>
        </div>
      </div>

      <div class="info-item">
        <div class="item-icon">
          <el-icon><Money /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">提现金额</div>
          <div class="item-value amount">{{ withdrawalRequest.amount }} USDT</div>
        </div>
      </div>

      <div class="info-item">
        <div class="item-icon">
          <el-icon><Wallet /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">提现地址</div>
          <div class="item-value">
            <el-space>
              <EllipticalLabel :value="withdrawalRequest.toAddress" :copy="false" />
              <CopyIcon :content="withdrawalRequest.toAddress" />
              <QrcodeIcon :value="withdrawalRequest.toAddress" />
            </el-space>
          </div>
        </div>
      </div>

      <div class="info-item">
        <div class="item-icon">
          <el-icon><Timer /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">申请时间</div>
          <div class="item-value"><Datetime :value="withdrawalRequest.createdAt" /></div>
        </div>
      </div>

      <div class="info-item" v-if="withdrawalRequest.txid">
        <div class="item-icon">
          <el-icon><Link /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">交易哈希</div>
          <div class="item-value"><EllipticalLabel :value="withdrawalRequest.txid" /></div>
        </div>
      </div>

      <div class="info-item" v-if="withdrawalRequest.rejectReason">
        <div class="item-icon">
          <el-icon><Warning /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">拒绝原因</div>
          <div class="item-value">{{ withdrawalRequest.rejectReason }}</div>
        </div>
      </div>

      <div class="info-item" v-if="withdrawalRequest.completedAt">
        <div class="item-icon">
          <el-icon><Check /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">完成时间</div>
          <div class="item-value"><Datetime :value="withdrawalRequest.completedAt" /></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { WithdrawalRequestDto } from '@/api/services'
  import { User, Money, Wallet, Timer, Link, Warning, Check } from '@element-plus/icons-vue'

  defineProps<{
    withdrawalRequest: WithdrawalRequestDto
  }>()
</script>

<style scoped lang="scss">
  .basic-info {
    .info-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
      gap: 16px;

      .info-item {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 16px;
        background: #f8f9fa;
        border-radius: 8px;
        transition: all 0.3s ease;

        &:hover {
          background: #e9ecef;
          transform: translateY(-2px);
          box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .item-icon {
          width: 40px;
          height: 40px;
          border-radius: 8px;
          background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
          display: flex;
          align-items: center;
          justify-content: center;
          flex-shrink: 0;

          .el-icon {
            font-size: 20px;
            color: white;
          }
        }

        .item-content {
          flex: 1;
          min-width: 0;

          .item-label {
            font-size: 12px;
            color: #666;
            margin-bottom: 4px;
            font-weight: 500;
          }

          .item-value {
            font-size: 14px;
            color: #333;
            font-weight: 500;
            word-break: break-all;

            &.amount {
              color: #e6a23c;
              font-weight: 600;
              font-size: 16px;
            }
          }
        }

        &:nth-child(1) .item-icon {
          background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        }

        &:nth-child(2) .item-icon {
          background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
        }

        &:nth-child(3) .item-icon {
          background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
        }

        &:nth-child(4) .item-icon {
          background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
        }

        &:nth-child(5) .item-icon {
          background: linear-gradient(135deg, #fa709a 0%, #fee140 100%);
        }

        &:nth-child(6) .item-icon {
          background: linear-gradient(135deg, #a8edea 0%, #fed6e3 100%);
        }

        &:nth-child(7) .item-icon {
          background: linear-gradient(135deg, #ffecd2 0%, #fcb69f 100%);
        }
      }
    }

    // 响应式设计
    @media (max-width: 768px) {
      .info-grid {
        grid-template-columns: 1fr;
      }
    }
  }
</style>
