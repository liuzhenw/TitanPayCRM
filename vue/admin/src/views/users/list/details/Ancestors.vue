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

  const getLevelTagType = (depth: number): 'success' | 'warning' | 'danger' | 'primary' => {
    const types: ('success' | 'warning' | 'danger' | 'primary')[] = [
      'success',
      'warning',
      'danger',
      'primary'
    ]
    return types[depth - 1] || 'primary'
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
      `确定要移除该用户的所有上级推荐关系吗?`,
      '确认移除',
      {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
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
