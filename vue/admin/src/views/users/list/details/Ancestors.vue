<template>
  <div class="superior-relations">
    <el-card shadow="never" v-loading="loading">
      <template #header>
        <div class="card-header">
          <span>上级关系</span>
          <el-button
            v-if="ancestors.length > 0"
            link
            size="small"
            type="danger"
            icon="Delete"
            :loading="actionLoading"
            @click="onRemoveAncestors"
          >
            移除上级
          </el-button>
          <el-button
            v-else
            link
            type="primary"
            size="small"
            icon="Plus"
            :loading="actionLoading"
            @click="onCreateRelation"
          >
            添加直推
          </el-button>
        </div>
      </template>

      <div v-if="ancestors.length > 0">
        <el-timeline>
          <el-timeline-item
            v-for="(ancestor, index) in ancestors"
            :key="ancestor.user.id"
            :timestamp="`第${ancestor.depth}层推荐人`"
            placement="top"
            :color="getTimelineColor(index)"
          >
            <div class="timeline-content">
              <div class="ancestor-info">
                <LevelTag v-if="ancestor.level" :value="ancestor.level" />
                <el-tag v-else type="info" size="small">普通用户</el-tag>
                <span class="ancestor-email">{{ ancestor.user.email }}</span>
              </div>
              <el-button
                type="primary"
                link
                size="small"
                icon="View"
                @click="viewReferrerDetails(ancestor.user.id)"
              >
                查看详情
              </el-button>
            </div>
          </el-timeline-item>
        </el-timeline>
      </div>

      <div v-else class="empty-state">
        <el-empty description="该用户暂无上级推荐人" />
      </div>
    </el-card>

    <CreateRelationDialog
      v-model="createVisible"
      v-model:loading="actionLoading"
      :user="user"
      @close="onCreateDialogClose"
    />
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRouter } from 'vue-router'
  import { ElMessage, ElMessageBox } from 'element-plus'
  import { ReferralRelationService, AncestorDto, UserDto } from '@/api/services'
  import LevelTag from '@/views/referrals/levelTag.vue'
  import CreateRelationDialog from './CreateRelationDialog.vue'

  const { user } = defineProps<{ user: UserDto }>()
  const emit = defineEmits<{
    (e: 'change'): void
  }>()
  const router = useRouter()

  const ancestors = ref<AncestorDto[]>([])
  const loading = ref(false)

  
  const getTimelineColor = (index: number): string => {
    const colors = [
      '#409EFF', // primary - L1
      '#67C23A', // success - L2
      '#E6A23C', // warning - L3
      '#F56C6C', // danger - L4
      '#909399' // info - L5+
    ]
    return colors[index] || colors[4]
  }

  const fetchData = async () => {
    loading.value = true
    try {
      const data = await ReferralRelationService.getAncestors(user.id)
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

  const actionLoading = ref(false)
  const onRemoveAncestors = async () => {
    const confirm = await ElMessageBox.confirm(
      `确定要移除该用户的所有上级以及下级用户的所有共同上级推荐关系吗?`,
      '确认移除',
      {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        confirmButtonClass: 'el-button--danger',
        type: 'warning'
      }
    )
    if (!confirm) return

    actionLoading.value = true
    try {
      await ReferralRelationService.removeAncestors(user.id)
      ElMessage.success('移除成功')
      emit('change')
      fetchData()
    } catch (error) {
      console.error('移除失败:', error)
      ElMessage.error('移除失败')
    } finally {
      actionLoading.value = false
    }
  }

  const createVisible = ref(false)
  const onCreateRelation = () => {
    createVisible.value = true
  }
  const onCreateDialogClose = async () => {
    emit('change')
    await fetchData()
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

    // 时间线内容样式
    .timeline-content {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 12px;

      .ancestor-info {
        display: inline-flex;
        align-items: center;
        gap: 8px;
        flex-wrap: wrap;
      }

      .ancestor-email {
        font-weight: 600;
        font-size: 14px;
        color: #303133;
      }
    }

    // 自定义时间线样式 - 紧凑布局
    :deep(.el-timeline) {
      .el-timeline-item {
        padding-bottom: 12px;

        &:last-child {
          padding-bottom: 0;
        }
      }

      .el-timeline-item__tail {
        left: 7px;
      }

      .el-timeline-item__node {
        left: 2px;
      }

      .el-timeline-item__timestamp {
        color: #606266;
        font-weight: 500;
        font-size: 13px;
        margin-bottom: 8px;
      }
    }

    .empty-state {
      padding: 40px 0;
      text-align: center;
    }
  }

  @media (max-width: 768px) {
    .superior-relations {
      .timeline-content {
        flex-direction: column;
        align-items: flex-start;
        gap: 8px;

        .el-button {
          align-self: flex-end;
        }
      }
    }
  }
</style>
