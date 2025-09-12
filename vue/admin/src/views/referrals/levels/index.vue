<template>
  <FullScreen>
    <div id="container">
      <el-card shadow="never" class="art-table-card">
        <ArtTableHeader @refresh="fetchData">
          <template #left>
            <el-button type="primary" @click="handleCreate">
              <el-icon><Plus /></el-icon>
              新建等级
            </el-button>
          </template>
        </ArtTableHeader>
        <ArtTable row-key="id" :data="tableData" :loading="dataLoading" :marginTop="10">
          <el-table-column key="name" prop="name" label="等级名称" />

          <el-table-column
            key="multiplier"
            prop="multiplier"
            label="佣金系数"
            width="120"
            align="right"
            sortable="custom"
          >
            <template #default="{ row }"> {{ (row.multiplier || 1) * 100 }}% </template>
          </el-table-column>
          <el-table-column
            key="userCount"
            prop="userCount"
            label="用户数量"
            width="120"
            align="right"
            sortable
          >
            <template #default="{ row }">
              {{ row.userCount || 0 }}
            </template>
          </el-table-column>
          <el-table-column
            key="totalCommission"
            prop="totalCommission"
            label="累计佣金"
            width="150"
            align="right"
            sortable
          >
            <template #default="{ row }">
              {{ formatAmount(row.totalCommission || 0) }}
            </template>
          </el-table-column>
          <el-table-column
            key="size"
            prop="size"
            label="等级大小"
            width="120"
            align="right"
            sortable
          />
          <el-table-column
            key="createdAt"
            prop="createdAt"
            label="创建时间"
            width="180"
            align="right"
            sortable
          >
            <template #default="{ row }">
              <Datetime :value="row.createdAt" />
            </template>
          </el-table-column>
          <el-table-column key="actions" width="150" align="right">
            <template #default="{ row }">
              <el-space>
                <ArtButtonTable type="edit" @click="handleEdit(row)" />
                <ArtButtonTable
                  type="delete"
                  @click="handleDelete(row)"
                  :disabled="row.userCount > 0"
                />
              </el-space>
            </template>
          </el-table-column>
        </ArtTable>
      </el-card>

      <!-- Level Update Drawer -->
      <LevelUpdateDrawer
        v-model="drawerVisible"
        :level="currentLevel"
        @success="handleDrawerSuccess"
      />
    </div>
  </FullScreen>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { ElMessage, ElMessageBox } from 'element-plus'
  import { ReferralLevelService, ReferralLevelDto } from '@/api/services'
  import LevelUpdateDrawer from './LevelUpdateDrawer.vue'
  import { Plus } from '@element-plus/icons-vue'

  const tableData = ref<ReferralLevelDto[]>([])
  const dataLoading = ref(false)
  const drawerVisible = ref(false)
  const currentLevel = ref<ReferralLevelDto>()

  const formatAmount = (amount: number) => {
    return amount.toFixed(2)
  }

  const fetchData = async () => {
    dataLoading.value = true
    try {
      const levels = await ReferralLevelService.getList()
      tableData.value = levels
    } catch (error) {
      console.error('获取推荐等级列表失败:', error)
      ElMessage.error('获取推荐等级列表失败')
    } finally {
      dataLoading.value = false
    }
  }

  const handleCreate = () => {
    currentLevel.value = undefined
    drawerVisible.value = true
  }

  const handleEdit = (row: ReferralLevelDto) => {
    currentLevel.value = row
    drawerVisible.value = true
  }

  const handleDelete = async (row: ReferralLevelDto) => {
    try {
      await ElMessageBox.confirm(
        `确定要删除推荐等级"${row.name}"吗？此操作不可撤销。`,
        '确认删除',
        {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }
      )

      await ReferralLevelService.delete(row.id)
      ElMessage.success('删除成功')
      fetchData()
    } catch (error) {
      if (error !== 'cancel') {
        console.error('删除推荐等级失败:', error)
        ElMessage.error('删除推荐等级失败')
      }
    }
  }

  const handleDrawerSuccess = () => {
    fetchData()
  }

  onMounted(() => {
    fetchData()
  })
</script>

<style scoped lang="scss">
  .art-table-card {
    border-radius: 8px;
    overflow: hidden;
  }
</style>
