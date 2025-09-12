<template>
  <el-card shadow="never">
    <template #header>
      <h4>系统设置</h4>
    </template>
    <div v-loading="loading" class="warp">
      <el-tabs v-model="tab" tab-position="left" style="height: 100%">
        <el-tab-pane
          v-for="[groupName, items] in settings"
          :key="groupName"
          :label="groupName"
          :name="groupName"
        >
          <SettingGroup :groupName="groupName" :items="items" />
        </el-tab-pane>
      </el-tabs>
    </div>
  </el-card>
</template>

<script setup lang="ts">
  import { SettingService } from '@/api/services/settings'
  import { SettingGroupsDto } from '@/api/services/settings/types'
  import SettingGroup from './group.vue'

  const loading = ref(false)
  const tab = ref('')
  const groups = ref<SettingGroupsDto>()
  const settings = computed(() => {
    if (!groups.value) return []
    return Object.entries(groups.value)
  })
  const fetchData = async () => {
    loading.value = true
    groups.value = await SettingService.getList().finally(() => (loading.value = false))
    tab.value = settings.value[0][0]
  }
  onMounted(async () => {
    fetchData()
  })
</script>

<style scoped lang="scss">
  .warp {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin: 0 auto;
    max-width: 800px;
  }
</style>
