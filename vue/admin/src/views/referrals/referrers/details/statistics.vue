<template>
  <div v-if="referrer.statistics && referrer.statistics.length > 0">
    <el-table :data="referrer.statistics" stripe>
      <el-table-column prop="product.name" label="产品名称" />
      <el-table-column prop="volume" label="销售量" align="right" width="100" />
      <el-table-column prop="revenue" label="销售额" align="right" width="120">
        <template #default="{ row }"> {{ row.revenue }} USD </template>
      </el-table-column>
      <el-table-column prop="commission" label="佣金" align="right" width="120">
        <template #default="{ row }"> {{ row.commission }} USD </template>
      </el-table-column>
    </el-table>
  </div>
  <div v-else class="empty-state">
    <el-empty description="暂无销售统计" />
  </div>
</template>

<script setup lang="ts">
  import { ReferrerWithDetailsDto } from '@/api/services'

  const { referrer } = defineProps<{
    referrer: ReferrerWithDetailsDto
  }>()
</script>

<style scoped lang="scss">
  .empty-state {
    padding: 40px 0;
    text-align: center;
  }
</style>
