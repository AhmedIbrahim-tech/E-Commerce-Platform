import { createSlice, createAsyncThunk, PayloadAction } from "@reduxjs/toolkit";
import { AxiosError } from "axios";
import { notificationService } from "@/services/notifications/notificationService";
import type { NotificationItem, NotificationListResponse } from "@/types";

interface ApiErrorResponse {
  message?: string;
  errors?: Record<string, string[]>;
}

interface NotificationState {
  items: NotificationItem[];
  unreadCount: number;
  loading: boolean;
  error: string | null;
  initialized: boolean;
  pageNumber: number;
  pageSize: number;
  totalCount: number;
}

const initialState: NotificationState = {
  items: [],
  unreadCount: 0,
  loading: false,
  error: null,
  initialized: false,
  pageNumber: 1,
  pageSize: 10,
  totalCount: 0,
};

function getErrorMessage(error: unknown): string {
  if (error instanceof AxiosError) {
    const responseData = error.response?.data as ApiErrorResponse | undefined;
    if (responseData?.message) return responseData.message;
    if (responseData?.errors) {
      const firstError = Object.values(responseData.errors)[0];
      if (firstError && firstError.length > 0) return firstError[0];
    }
    return error.message || "An error occurred";
  }
  if (error instanceof Error) return error.message;
  return "An error occurred";
}

export const fetchNotificationsAsync = createAsyncThunk(
  "notifications/fetch",
  async (
    params: { pageNumber?: number; pageSize?: number; force?: boolean } | undefined,
    { rejectWithValue }
  ) => {
    try {
      const pageNumber = params?.pageNumber ?? 1;
      const pageSize = params?.pageSize ?? 10;
      const result = await notificationService.getNotifications(pageNumber, pageSize);
      return { ...result, pageNumber, pageSize };
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  },
  {
    condition: (params, { getState }) => {
      const state = getState() as { notifications: NotificationState };
      if (state.notifications.loading) return false;
      if (!params?.force && state.notifications.initialized) return false;
      return true;
    },
  }
);

export const markNotificationReadAsync = createAsyncThunk(
  "notifications/markRead",
  async (id: string, { rejectWithValue }) => {
    try {
      await notificationService.markRead(id);
      return id;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

export const markAllNotificationsReadAsync = createAsyncThunk(
  "notifications/markAllRead",
  async (_, { rejectWithValue }) => {
    try {
      await notificationService.markAllRead();
      return true;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  }
);

const notificationSlice = createSlice({
  name: "notifications",
  initialState,
  reducers: {
    resetNotifications: (state) => {
      state.items = [];
      state.unreadCount = 0;
      state.loading = false;
      state.error = null;
      state.initialized = false;
      state.pageNumber = 1;
      state.pageSize = 10;
      state.totalCount = 0;
    },
    upsertIncomingNotification: (state, action: PayloadAction<NotificationItem>) => {
      const incoming = action.payload;
      const idx = state.items.findIndex((n) => n.id === incoming.id);
      if (idx === -1) {
        state.items = [incoming, ...state.items];
      } else {
        state.items[idx] = incoming;
      }
      if (!incoming.isRead) {
        state.unreadCount = Math.max(state.unreadCount, 1);
      }
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchNotificationsAsync.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchNotificationsAsync.fulfilled, (state, action) => {
        state.loading = false;
        const page: NotificationListResponse = action.payload.page;
        state.items = page.data || [];
        state.unreadCount = action.payload.unreadCount ?? 0;
        state.pageNumber = action.payload.pageNumber;
        state.pageSize = action.payload.pageSize;
        state.totalCount = page.totalCount || 0;
        state.initialized = true;
        state.error = null;
      })
      .addCase(fetchNotificationsAsync.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(markNotificationReadAsync.fulfilled, (state, action) => {
        const id = action.payload;
        const idx = state.items.findIndex((n) => n.id === id);
        if (idx !== -1 && !state.items[idx].isRead) {
          state.items[idx] = { ...state.items[idx], isRead: true };
          state.unreadCount = Math.max(0, state.unreadCount - 1);
        }
      })
      .addCase(markAllNotificationsReadAsync.fulfilled, (state) => {
        state.items = state.items.map((n) => ({ ...n, isRead: true }));
        state.unreadCount = 0;
      });
  },
});

export const { resetNotifications, upsertIncomingNotification } = notificationSlice.actions;
export default notificationSlice.reducer;

