<template>
  <div>
    <ArtTable row-key="id" :data="tableData" :loading="dataLoading">
      <el-table-column key="level" prop="levelId" label="用户等级" width="110" sortable>
        <template #default="{ row }">
          <LevelTag :value="row.level" />
        </template>
      </el-table-column>
      <el-table-column key="email" prop="email" label="上级用户" show-overflow-tooltip>
        <template #default="{ row }">
          {{ row.user.email }}
        </template>
      </el-table-column>
      <el-table-column
        key="depth"
        prop="depth"
        label="层级"
        width="80"
        sortable="custom"
        align="right"
      >
        <template #default="{ row }">
          {{ row.depth }}
        </template>
      </el-table-column>
      <el-table-column
        key="totalCommission"
        prop="totalCommission"
        label="累计佣金"
        width="120"
        align="right"
      >
        <template #default="{ row }"> {{ row.totalCommission }} USD </template>
      </el-table-column>
      <el-table-column key="createdAt" prop="createdAt" label="创建时间" align="right">
        <template #default="{ row }">
          <Datetime :value="row.createdAt" />
        </template>
      </el-table-column>
      <el-table-column key="actions" width="80" align="right">
        <template #default="{ row }">
          <el-space>
            <ArtButtonTable type="view" @click="toUserDetails(row.user.id)" />
          </el-space>
        </template>
      </el-table-column>
    </ArtTable>
  </div>
</template>

<script setup lang="ts">
  import { ReferralRelationService, AncestorDto } from '@/api/services'
  import LevelTag from '../../levelTag.vue'

  const router = useRouter()
  const { userId } = defineProps<{
    userId: string
  }>()

  const tableData = ref<AncestorDto[]>([])
  const dataLoading = ref(false)
  const fetchData = async () => {
    dataLoading.value = true
    tableData.value = await ReferralRelationService.getAncestors(userId).finally(
      () => (dataLoading.value = false)
    )
  }

  const toUserDetails = (id: string) => {
    router.push(`/users/${id}/details`)
  }

  watchEffect(() => {
    if (userId) {
      fetchData()
    } else {
      tableData.value = []
    }
  })
</script>

<style scoped lang="scss"></style>
