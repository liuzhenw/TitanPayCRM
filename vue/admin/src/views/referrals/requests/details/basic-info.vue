<template>
  <div class="basic-info">
    <div class="info-grid">
      <div class="info-item">
        <div class="item-icon">
          <el-icon><User /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">申请用户</div>
          <div class="item-value">
            <div class="user-info">
              <div class="user-name">{{ referrerRequest.user.name }}</div>
              <div class="user-email">{{ referrerRequest.user.email }}</div>
            </div>
          </div>
        </div>
      </div>

      <div class="info-item">
        <div class="item-icon">
          <el-icon><Avatar /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">申请等级</div>
          <div class="item-value level">{{ referrerRequest.level.name }}</div>
        </div>
      </div>

      <div class="info-item">
        <div class="item-icon">
          <el-icon><Timer /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">申请时间</div>
          <div class="item-value"><Datetime :value="referrerRequest.createdAt" /></div>
        </div>
      </div>

      <div class="info-item" v-if="referrerRequest.updatedAt">
        <div class="item-icon">
          <el-icon><Check /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">处理时间</div>
          <div class="item-value"><Datetime :value="referrerRequest.updatedAt" /></div>
        </div>
      </div>

      <div class="info-item" v-if="referrerRequest.rejectReason">
        <div class="item-icon">
          <el-icon><Warning /></el-icon>
        </div>
        <div class="item-content">
          <div class="item-label">拒绝原因</div>
          <div class="item-value reject-reason">{{ referrerRequest.rejectReason }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ReferrerRequestDto } from '@/api/services'
  import { User, Avatar, Timer, Check, Warning } from '@element-plus/icons-vue'

  defineProps<{
    referrerRequest: ReferrerRequestDto
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

            &.level {
              color: #409eff;
              font-weight: 600;
              font-size: 16px;
            }

            &.reject-reason {
              color: #f56c6c;
              font-weight: 500;
            }

            .user-info {
              .user-name {
                font-weight: 600;
                margin-bottom: 2px;
              }

              .user-email {
                font-size: 12px;
                color: #666;
              }
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
