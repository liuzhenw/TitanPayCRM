<template>
  <div class="recommendees">
    <ArtTable
      row-key="id"
      :data="tableData"
      :loading="dataLoading"
      :marginTop="10"
      height="400"
      v-model:pagination="pagination"
      @pagination:current-change="onPaginationCurrentChange"
      @pagination:size-change="onPaginationSizeChange"
      @sort-change="onSortChange"
    >
      <el-table-column key="level" prop="levelId" label="用户等级" width="110" sortable="custom">
        <template #default="{ row }">
          <LevelTag :value="row.level" />
        </template>
      </el-table-column>
      <el-table-column key="email" prop="email" label="下级用户" show-overflow-tooltip>
        <template #default="{ row }">
          {{ row.email }}
        </template>
      </el-table-column>
      <el-table-column
        key="recommender"
        prop="recommender.email"
        label="直接推荐人"
        show-overflow-tooltip
      >
        <template #default="{ row }">
          {{ row.recommender.email }}
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
      <el-table-column
        key="createdAt"
        prop="createdAt"
        label="创建时间"
        align="right"
        sortable="custom"
        show-overflow-tooltip
      >
        <template #default="{ row }">
          <Datetime :value="row.createdAt" />
        </template>
      </el-table-column>
      <el-table-column key="actions" width="80" align="right">
        <template #default="{ row }">
          <el-space>
            <ArtButtonTable
              type="view"
              :disabled="row.level == null"
              @click="toReferrerDetails(row.id)"
            />
          </el-space>
        </template>
      </el-table-column>
    </ArtTable>

    <div v-if="tableData.length === 0 && !dataLoading" class="empty-state">
      <el-empty description="暂无推荐用户" />
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, reactive, onMounted, watch } from 'vue'
  import {
    ReferrerService,
    RecommendeeDto,
    RecommendeeQueryInput,
    ReferrerDto
  } from '@/api/services'
  import LevelTag from '../../levelTag.vue'
  import { SearchFormItem } from '@/types'

  const props = defineProps<{
    referrer: ReferrerDto
  }>()

  const router = useRouter()
  const tableData = ref<RecommendeeDto[]>([])
  const filter = reactive<RecommendeeQueryInput>({
    ancestorId: props.referrer.id
  })
  const filterItems: SearchFormItem[] = [
    {
      label: '邮箱地址',
      prop: 'id',
      type: 'userSearch',
      config: {
        clearable: true
      }
    }
  ]

  const pagination = reactive({
    current: 1,
    size: 10,
    total: 0,
    disableScrollToTop: true
  })
  const dataLoading = ref(false)

  const fetchData = async (input: RecommendeeQueryInput) => {
    dataLoading.value = true
    try {
      const res = await ReferrerService.getRecommendees(input)
      pagination.total = res.totalCount
      tableData.value = res.items
    } finally {
      dataLoading.value = false
    }
  }

  const onPaginationCurrentChange = (val: number) => {
    pagination.current = val
    onPaginationChange()
  }

  const onPaginationSizeChange = (val: number) => {
    pagination.size = val
    pagination.current = 1
    onPaginationChange()
  }

  const onPaginationChange = () => {
    filter.maxResultCount = pagination.size
    filter.skipCount = (pagination.current - 1) * pagination.size
    filter.ancestorId = props.referrer.id
    fetchData(filter)
  }

  const onSortChange = (data: { column: any; prop: string; order: any }) => {
    if (!data.prop || !data.order) {
      filter.sorting = undefined
    } else {
      const field = data.prop[0].toUpperCase() + data.prop.slice(1)
      const dire = data.order === 'ascending' ? 'asc' : 'desc'
      filter.sorting = `${field} ${dire}`
    }
    pagination.current = 1
    fetchData(filter)
  }

  const refreshData = () => {
    pagination.current = 1
    fetchData(filter)
  }

  const toReferrerDetails = (id: string) => {
    router.push(`/referrals/referrers/${id}`)
  }

  watch(
    () => props.referrer.id,
    (newId) => {
      if (newId) {
        filter.ancestorId = newId
        refreshData()
      }
    },
    { immediate: true }
  )

  onMounted(() => {
    if (props.referrer.id) {
      fetchData(filter)
    }
  })
</script>

<style scoped lang="scss"></style>
