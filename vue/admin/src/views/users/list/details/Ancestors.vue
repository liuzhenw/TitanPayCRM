<template>
  <div class="superior-relations">
    <el-card shadow="never" v-loading="loading">
      <template #header>
        <div class="card-header">
          <span>上级推荐关系</span>
        </div>
      </template>

      <div v-if="ancestors.length > 0" class="ancestors-list">
        <div v-for="(ancestor, index) in ancestors" :key="ancestor.user.id" class="ancestor-item">
          <div class="ancestor-level">
            <el-tag :type="getLevelTagType(index)" size="small"> L{{ ancestor.depth }} </el-tag>
          </div>
          <div class="ancestor-info">
            <div class="ancestor-name">{{ ancestor.user.email }}</div>
            <div class="ancestor-details">
              <span class="level-badge">
                <LevelTag v-if="ancestor.level" :value="ancestor.level" />
                <el-tag v-else type="info"> 普通用户 </el-tag>
              </span>

              <span class="commission-info">
                佣金: ${{ formatAmount(ancestor.totalCommission) }}
              </span>
            </div>
          </div>
          <div class="ancestor-actions">
            <el-button
              type="primary"
              link
              size="small"
              @click="viewReferrerDetails(ancestor.user.id)"
            >
              查看详情
            </el-button>
          </div>
        </div>
      </div>

      <div v-else class="empty-state">
        <el-empty description="该用户暂无上级推荐人" />
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRouter } from 'vue-router'
  import { ElMessage } from 'element-plus'
  import { ReferralRelationService, AncestorDto } from '@/api/services'
  import LevelTag from '@/views/referrals/levelTag.vue'
  import { Refresh } from '@element-plus/icons-vue'

  interface Props {
    userId: string
  }

  const props = defineProps<Props>()
  const router = useRouter()

  const ancestors = ref<AncestorDto[]>([])
  const loading = ref(false)

  const formatAmount = (amount: number) => {
    return amount.toFixed(2)
  }

  const getLevelTagType = (depth: number): 'success' | 'warning' | 'danger' | 'info' => {
    const types: ('success' | 'warning' | 'danger' | 'info')[] = [
      'success',
      'warning',
      'danger',
      'info'
    ]
    return types[depth - 1] || 'info'
  }

  const fetchData = async () => {
    loading.value = true
    try {
      const data = await ReferralRelationService.getAncestors(props.userId)
      // 按 Depth 从小到大排列 (L1, L2, L3...)
      ancestors.value = data.sort((a, b) => a.depth - b.depth)
    } catch (error) {
      console.error('获取上级推荐关系失败:', error)
      ElMessage.error('获取上级推荐关系失败')
    } finally {
      loading.value = false
    }
  }

  const viewReferrerDetails = (userId: string) => {
    router.push(`/referrals/referrers/${userId}`)
  }

  onMounted(() => {
    fetchData()
  })
</script>

<style scoped lang="scss">
  .superior-relations {
    .card-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .ancestors-list {
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .ancestor-item {
      display: flex;
      align-items: center;
      gap: 16px;
      padding: 16px;
      border: 1px solid #e4e7ed;
      border-radius: 8px;
      background: #fafafa;
      transition: all 0.3s ease;

      &:hover {
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        background: #fff;
      }
    }

    .ancestor-level {
      flex-shrink: 0;
    }

    .ancestor-info {
      flex: 1;
      min-width: 0;
    }

    .ancestor-name {
      font-weight: 600;
      font-size: 14px;
      color: #303133;
      margin-bottom: 4px;
    }

    .ancestor-details {
      display: flex;
      align-items: center;
      gap: 12px;
      flex-wrap: wrap;
      font-size: 12px;
      color: #909399;

      .level-badge {
        flex-shrink: 0;
      }

      .commission-info {
        color: #67c23a;
        font-weight: 500;
      }
    }

    .ancestor-actions {
      flex-shrink: 0;
    }

    .empty-state {
      padding: 40px 0;
      text-align: center;
    }
  }

  @media (max-width: 768px) {
    .superior-relations {
      .ancestor-item {
        flex-direction: column;
        align-items: flex-start;
        gap: 12px;
      }

      .ancestor-details {
        flex-direction: column;
        align-items: flex-start;
        gap: 8px;
      }

      .ancestor-actions {
        align-self: flex-end;
      }
    }
  }
</style>
